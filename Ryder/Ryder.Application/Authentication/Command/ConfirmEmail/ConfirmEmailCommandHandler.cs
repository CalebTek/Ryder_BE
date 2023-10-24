using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Utility;

namespace Ryder.Application.Authentication.Command.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationContext _context;

        public ConfirmEmailCommandHandler(UserManager<AppUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {

            if (request.Email == null || request.Token == null)
            {
                return await Result.FailAsync("Invalid Data Input");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return await Result.FailAsync("User does not exist");
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                _context.Update(user);
                _context.SaveChanges();
                return await Result.SuccessAsync("Your Email has been confirmed");
            }
            else
            {
                return await Result.FailAsync("Unable to confirm your email at the moment!");
            }
        }
    }
}