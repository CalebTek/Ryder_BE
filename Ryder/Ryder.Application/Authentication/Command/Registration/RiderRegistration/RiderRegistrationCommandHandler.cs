using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Policy;
using Ryder.Infrastructure.Utility;
using System.Transactions;

namespace Ryder.Application.Authentication.Command.Registration.RiderRegistration
{
    public class RiderRegistrationCommandHandler : IRequestHandler<RiderRegistrationCommand, IResult>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;
        private readonly IDocumentUploadService _uploadService;

        public RiderRegistrationCommandHandler(ApplicationContext context, UserManager<AppUser> userManager,
            ISmtpEmailService emailService, IDocumentUploadService uploadService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _uploadService = uploadService;
        }

        public async Task<IResult> Handle(RiderRegistrationCommand request,  CancellationToken cancellationToken)
        {
            //Perform logic for sign up as a Rider
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return Result.Fail("Rider exist");
            
            user = new Domain.Entities.AppUser()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                Address = new Address
                {
                    City = request.City,
                    State = request.State,
                    Country = request.Country,
                    Longitude = request.Longitude,
                    Latitude = request.Latitude
                }
            };

            var validIdUpload = await _uploadService.DocumentUploadAsync(request.ValidIdUrl);
            if (validIdUpload == null) return Result.Fail("Failed to upload valid Id");
            var passportPhotoUpload = await _uploadService.PhotoUploadAsync(request.PassportPhoto);
            if (passportPhotoUpload == null) return Result.Fail("Failed to upload passport photo");
            var bikeDocumentUpload = await _uploadService.DocumentUploadAsync(request.BikeDocument);
            if (bikeDocumentUpload == null) return Result.Fail("Failed to upload bike document");

            var riderDocumentation = new Domain.Entities.Rider()
            {
                ValidIdUrl = validIdUpload.SecureUrl.ToString(),
                PassportPhoto = passportPhotoUpload.SecureUrl.ToString(),
                BikeDocument = bikeDocumentUpload.SecureUrl.ToString(),
                AppUserId = user.Id
            };

            //Perform transaction and save to Db
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdRider = await _userManager.CreateAsync(user, request.Password);
                if (!createdRider.Succeeded) return Result.Fail();
                var CreateRole = await _userManager.AddToRoleAsync(user, Policies.Rider);
                if (!CreateRole.Succeeded) return Result.Fail();
                await _context.Riders.AddAsync(riderDocumentation, cancellationToken);

                //Send Confirmation mail
                var token = new Random().Next(100000, 999999).ToString();

                //TODO:Change this to an email template
                var emailBody =
                    $"Your confirmation code is {token}. Please enter this code on our website to confirm your email address. This code will expire in 10 minutes.";
                var sent = await _emailService.SendEmailAsync(user.Email, "Confirm Email", emailBody);

                if (!sent)
                    return await Result.FailAsync("Error sending email");


                await _context.SaveChangesAsync(cancellationToken);
                transaction.Complete();

                return Result.Success("Signup successful");
            }
        }
    }
}