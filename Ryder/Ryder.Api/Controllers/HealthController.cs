using System.Reflection;
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

        [HttpGet("logs")]
        public IActionResult Logs()
        {
            string executablePath =
                System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var text = System.IO.File.ReadAllText($"{executablePath}/logs/rider-logs-{DateTime.Now:yyyy-MM-dd}.log");
            return Content(text, "text/plain");
        }

        [HttpPut("clearlogs")]
        public IActionResult ClearLogs()
        {
            System.IO.File.WriteAllText($"/logs/rider-logs-{DateTime.Now:yyyy-MM-dd}.log",
                string.Empty);
            return Ok("Success");
        }
    }
}