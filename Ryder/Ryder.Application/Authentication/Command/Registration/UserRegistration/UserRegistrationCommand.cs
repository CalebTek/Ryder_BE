using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;

namespace Ryder.Application.Authentication.Command.Registration.UserRegistration
{
    public class UserRegistrationCommand : IRequest<IResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationCommandValidator()
        {
            //Validate properties
            RuleFor(x => x.FirstName).NotEmpty().Matches("^[A-Z][a-zA-Z]*$").WithMessage("Invalid name Format");
            RuleFor(x => x.LastName).NotEmpty().Matches("^[A-Z][a-zA-Z]*$").WithMessage("Invalid name Format");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email address Format");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(11).WithMessage("Invalid Phone number");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
                .WithMessage("Invalid Password, must have 'ABCabcd723#$@'");
        }
    }
}