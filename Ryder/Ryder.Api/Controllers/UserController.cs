using Microsoft.AspNetCore.Mvc;
using Ryder.Application.User.Command.EditUserProfile;
using Ryder.Application.User.Query.GetCurrentUser;

namespace Ryder.Api.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await Initiate(() => Mediator.Send(new GetCurrentUserCommand()));
        }

        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] ProfileModel profileUpdate)
        {
            return await Initiate(() => Mediator.Send(new EditUserProfileComand(userId, profileUpdate)));
        }
    }
}