using MediatR;
using AspNetCoreHero.Results;
using FluentValidation;

namespace Ryder.Application.Authentication.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest<IResult<ResetPasswordResponse>>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ResetToken).NotEmpty();
        }
    }
}