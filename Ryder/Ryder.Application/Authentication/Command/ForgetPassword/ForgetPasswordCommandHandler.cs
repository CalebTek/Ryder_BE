using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using Ryder.Infrastructure.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net;
using Serilog;

namespace Ryder.Application.Authentication.Command.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, IResult<ForgetPasswordResponse>>
    {
        private readonly ISmtpEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public ForgetPasswordCommandHandler(ISmtpEmailService emailService, IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _emailService = emailService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IResult<ForgetPasswordResponse>> Handle(ForgetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Find the user by their email address
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result<ForgetPasswordResponse>.Fail("User not found.");

                // Generate a password reset token for the user
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Create a password reset link with the token
                var appUrl = _configuration["AppUrl"];
                var resetLink = $"{appUrl}/reset-password?token={WebUtility.UrlEncode(passwordResetToken)}&email={request.Email}";

                // Send a password reset email using your IEmailService
                var emailSubject = "Forgot Password";
                var emailMessage = $"Click the link below to reset your password:\n{resetLink}";

                // Use your email service to send the email
                var emailSent = await _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

                if (!emailSent)
                    return Result<ForgetPasswordResponse>.Fail("Failed to send password reset email.");

                return Result<ForgetPasswordResponse>.Success("Password reset email sent successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");
                return Result<ForgetPasswordResponse>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}