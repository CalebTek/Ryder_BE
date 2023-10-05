using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ryder.Api.Controllers
{
    [AllowAnonymous]
    public class HealthController : ApiController
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpGet("error/500")]
        public IActionResult GetThrow500()
        {
            throw new Exception();
        }

        [HttpGet("error/BadRequest400")]
        public IActionResult GetThrowBadRequest40()
        {
            return new BadRequestResult();
        }
    }
}