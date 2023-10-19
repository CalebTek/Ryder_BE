using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Policy;
using Ryder.Infrastructure.Utility;
using System.Net;
using System.Transactions;

namespace Ryder.Application.Authentication.Command.Registration.RiderRegistration
{
    public class RiderRegistrationCommandHandler : IRequestHandler<RiderRegistrationCommand, IResult>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;
        private readonly IDocumentUploadService _uploadService;
        private readonly IConfiguration _configuration;

        public RiderRegistrationCommandHandler(ApplicationContext context, UserManager<AppUser> userManager,
            ISmtpEmailService emailService, IDocumentUploadService uploadService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _uploadService = uploadService;
            _configuration = configuration;
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
                    PostCode = request.PostCode,
                    Country = request.Country,
                    Longitude = request.Longitude,
                    Latitude = request.Latitude,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                AvailabilityStatus = RiderAvailabilityStatus.Unavailable,
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

                //Sending Email token to verify the users email.
                var verifyEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var appUrl = _configuration["AppUrl"];
                var resetLink = $"{appUrl}/confirmation?token={WebUtility.UrlEncode(verifyEmailToken)}&email={WebUtility.UrlEncode(request.Email)}";

                var emailSubject = "Verify Email";
                var emailMessage = $"Click the link below to Verify your email:\n{resetLink}";

                var emailSent = await _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

                if (!emailSent)
                    return await Result.FailAsync("Error sending email");

                await _context.SaveChangesAsync(cancellationToken);
                transaction.Complete();

                return Result.Success("Signup successful");
            }
        }
    }
}