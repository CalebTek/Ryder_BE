using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Policy;
using Ryder.Infrastructure.Utility;
using System.Transactions;

namespace Ryder.Application.Authentication.Command.Registration.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, IResult>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpEmailService _emailService;

        public UserRegistrationCommandHandler(ApplicationContext context, UserManager<AppUser> userManager,
            ISmtpEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IResult> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            //Perform logic for sign up as a User
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return Result.Fail("User exist");
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

                //Send Confirmation mail
               /* var token = new Random().Next(100000, 999999).ToString();*/
               var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //TODO:Change this to an email template
                var emailBody =
                    $"Your confirmation link is {token}. Please enter this code on our website to confirm your email address. This link will expire in 10 minutes.";
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