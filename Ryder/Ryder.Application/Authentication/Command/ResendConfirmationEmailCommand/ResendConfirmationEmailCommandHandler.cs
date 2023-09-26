using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Utility;

namespace Ryder.Application.Authentication.Command.ResendConfirmationEmailCommand
{
    public class
        ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;

        public ResendConfirmationEmailCommandHandler(UserManager<AppUser> userManager, ISmtpEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IResult> Handle(ResendConfirmationEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return await Result.FailAsync("User does not exist");

            //Send Confirmation mail
            var token = new Random().Next(100000, 999999).ToString();

            // Send an email to the user's email address with the confirmation code
            var emailBody =
                $"Your confirmation code is {token}. Please enter this code on our website to confirm your email address. This code will expire in 10 minutes.";
            var sent = await _emailService.SendEmailAsync(user.Email, "Confirm Email", emailBody);

            if (!sent)
                return await Result.FailAsync("Error sending email");
            await _userManager.UpdateAsync(user);
            return await Result.SuccessAsync("Confirmation email successfully sent");
        }
    }
}