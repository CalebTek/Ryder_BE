using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ryder.Application.User.Command.EditUserProfile;
using Ryder.Application.User.Query.GetCurrentUser;
using Ryder.Application.User.Query.GetUserInfo;

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

        [HttpGet("UserInformation/{userId}")]
        public async Task<IActionResult> UserInformation(string userId)
        {
            return await Initiate(() => Mediator.Send(new GetUserInfoQuery(userId)));
        }
    }
}