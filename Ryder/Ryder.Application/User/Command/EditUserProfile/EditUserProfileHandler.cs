using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;

namespace Ryder.Application.User.Command.EditUserProfile
{
    public class EditUserProfileHandler : IRequestHandler<EditUserProfileComand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditUserProfileHandler(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task<IResult> Handle(EditUserProfileComand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null) return Result.Fail("User does not exist");

                user.Id = Guid.Parse(request.UserId);
                user.FirstName = request.ProfileModel.FirstName;
                user.LastName = request.ProfileModel.LastName;
                user.Email = request.ProfileModel.Email;
                user.PhoneNumber = request.ProfileModel.UserPhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded
                    ? Result.Success("Update successful")
                    : Result.Fail("Oops Something Went Wrong");
            }
            catch (Exception ex)
            {
                return Result.Fail("Oops Something Went Wrong " + ex.Message);
            }
        }
    }
}