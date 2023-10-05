using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Serilog;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult<ResetPasswordResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager, ISmtpEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IResult<ResetPasswordResponse>> Handle(ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate the reset token
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result<ResetPasswordResponse>.Fail("User not found.");

                var decodedToken = WebUtility.UrlDecode(request.ResetToken);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
                if (!result.Succeeded)
                    return Result<ResetPasswordResponse>.Fail("Failed to reset the password.");

                // Check if the new password and confirm password match
                if (request.NewPassword != request.ConfirmPassword)
                    return Result<ResetPasswordResponse>.Fail("New password and confirm password do not match.");

                // Send a password reset email using your IEmailService
                var emailSubject = "Password Reset Successful";
                var emailMessage = "Your password has been successfully reset.";

                var emailSent = await _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

                if (!emailSent)
                    return Result<ResetPasswordResponse>.Fail("Failed to send password reset email.");

                var response = new ResetPasswordResponse
                {
                    Email = request.Email,
                    NewPassword = request.NewPassword,
                    ConfirmPassword = request.ConfirmPassword,
                    ResetToken = request.ResetToken
                };

                return Result<ResetPasswordResponse>.Success(response, "Password reset successful.");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");
                return Result<ResetPasswordResponse>.Fail("An error occurred while resetting the password.");
            }
        }
    }
}
