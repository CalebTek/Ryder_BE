using Microsoft.AspNetCore.Mvc;
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
    }
}