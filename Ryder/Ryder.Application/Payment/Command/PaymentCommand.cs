using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using PayStack.Net;

namespace Ryder.Application.Payment.Command
{
    public class PaymentCommand : IRequest<IResult<PaymentResponse>>
    {
        public int AmountInKobo { get; set; }
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
        public string Currency { get; set; } = "NGN";
    }

    public class PaymentCommandValidator : AbstractValidator<PaymentCommand>
    {
        public PaymentCommandValidator()
        {
            RuleFor(r => r.AmountInKobo)
                .GreaterThan(0)
                .WithMessage("AmountInKobo must be greater than 0");

            RuleFor(r => r.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(r => r.CallbackUrl)
                .NotNull()
                .NotEmpty()
                .Must(BeAValidUri)
                .WithMessage("Invalid URL format");
        }

        private bool BeAValidUri(string callbackUrl)
        {
            return Uri.TryCreate(callbackUrl, UriKind.Absolute, out _);
        }
    }
}
