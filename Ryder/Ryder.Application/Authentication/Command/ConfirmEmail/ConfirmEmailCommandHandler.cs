using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Utility;

namespace Ryder.Application.Authentication.Command.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return await Result.FailAsync("User does not exist");
            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);
            return await Result.SuccessAsync($"{user.Email} successfully confirmed");
        }
    }
}