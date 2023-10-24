using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ryder.Application.Authentication.Command.ForgetPassword;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Utility;
using Serilog;
using System.Net;

namespace Ryder.Application.Authentication.Command.ResendConfirmationEmailCommand
{
    public class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ResendConfirmationEmailCommandHandler(UserManager<AppUser> userManager, ISmtpEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IResult> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return Result<ResendConfirmationEmailResponse>.Fail($"User with email {request.Email} not found.");

                var verifyEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var appUrl = _configuration["AppUrl"];
                var resetLink = $"{appUrl}/confirmation?token={WebUtility.UrlEncode(verifyEmailToken)}&email={WebUtility.UrlEncode(request.Email)}";

                var emailSubject = "Verify Email";
                var emailMessage = $"Click the link below to Verify your email:\n{resetLink}";

                var emailSent = await _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

                if (!emailSent)
                    return Result<ResendConfirmationEmailResponse>.Fail("Failed to send confirmation email.");

                return Result<ResendConfirmationEmailResponse>.Success("Confirmation email sent successfully.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");
                return Result<ResendConfirmationEmailResponse>.Fail("An error occurred while processing your request.");
            }
        }
    }
}