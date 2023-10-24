using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Policy;
using Ryder.Infrastructure.Utility;
using System.Net;
using System.Transactions;

namespace Ryder.Application.Authentication.Command.Registration.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, IResult>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserRegistrationCommandHandler(ApplicationContext context, UserManager<AppUser> userManager,
            ISmtpEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IResult> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            //Perform logic for sign up as a User
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return Result.Fail("A User with this Email exist.");
            user = new AppUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                RefreshToken = Guid.NewGuid().ToString(),
            };

            //Perform transaction
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdUser = await _userManager.CreateAsync(user, request.Password);
                if (!createdUser.Succeeded) return Result.Fail();
                var CreateRole = await _userManager.AddToRoleAsync(user, Policies.Customer);
                if (!CreateRole.Succeeded) return Result.Fail();

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

                return Result.Success("Signup successful.");
            }
        }
    }
}