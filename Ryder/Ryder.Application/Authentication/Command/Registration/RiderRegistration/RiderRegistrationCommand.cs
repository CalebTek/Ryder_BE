using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Ryder.Application.Authentication.Command.Registration.RiderRegistration
{
    public class RiderRegistrationCommand : IRequest<IResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
        public IFormFile ValidIdUrl { get; set; }
        public IFormFile PassportPhoto { get; set; }
        public IFormFile BikeDocument { get; set; }
    }

    public class RiderRegistrationCommandValidator : AbstractValidator<RiderRegistrationCommand>
    {
        public RiderRegistrationCommandValidator()
        {
            //Validate properties
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Matches("^[A-Z][a-zA-Z]*$")
                .WithMessage("Invalid name Format");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Matches("^[A-Z][a-zA-Z]*$")
                .WithMessage("Invalid name Format");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid Email address Format");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().MinimumLength(11)
                .MaximumLength(11)
                .WithMessage("Invalid Phone number");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Invalid Password, must have 'ABCabcd723#$@'");
            RuleFor(x => x.ValidIdUrl)
                .NotNull().NotEmpty()
                .WithMessage("Id cannot be empty");
            RuleFor(x => x.BikeDocument)
                .NotEmpty()
                .NotNull()
                .WithMessage("Document cannot be empty");
            RuleFor(x => x.PassportPhoto).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
            RuleFor(x => x.Longitude).NotEmpty();
            RuleFor(x => x.PostCode).NotEmpty();
        }
    }
}