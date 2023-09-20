using MediatR;
using AspNetCoreHero.Results;
using FluentValidation;

namespace Ryder.Application.Authentication.Command.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<IResult<ForgetPasswordResponse>>
    {
        public string Email { get; set; }
    }

    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}