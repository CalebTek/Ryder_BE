using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ryder.Application.Authentication.Command.ConfirmEmail;
using Ryder.Application.Authentication.Command.ForgetPassword;
using Ryder.Application.Authentication.Command.Login;
using Ryder.Application.Authentication.Command.Logout;
using Ryder.Application.Authentication.Command.Registration.RiderRegistration;
using Ryder.Application.Authentication.Command.Registration.UserRegistration;
using Ryder.Application.Authentication.Command.ResendConfirmationEmailCommand;
using Ryder.Application.Authentication.Command.ResetPassword;

namespace Ryder.Api.Controllers
{
    public class AuthenticationController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserRegistrationCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("CreateRider")]
        public async Task<IActionResult> CreateRider([FromForm] RiderRegistrationCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand request)
        {
            return await Initiate(() => Mediator.Send(request));
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
        {
            return await Initiate(() => Mediator.Send(request));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("SendConfirmEmail")]
        public async Task<IActionResult> SendConfirmEmail([FromBody] ResendConfirmationEmailCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            return await Initiate(() => Mediator.Send(new LogoutCommand()));
        }
    }
}