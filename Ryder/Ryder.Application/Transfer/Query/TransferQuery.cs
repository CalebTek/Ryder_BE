using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;

namespace Ryder.Application.Transfer.Query
{
    public class TransferQuery : IRequest<IResult<TransferQueryResponse>>
    {
        public string TransferCode { get; set; }
        public string Otp { get; set; }
        public Guid RiderId { get; set; }
    }

    public class TransferQueryValidator : AbstractValidator<TransferQuery>
    {
        public TransferQueryValidator()
        {
            // Rule for the TransferCode property
            RuleFor(x => x.TransferCode)
                .NotEmpty().WithMessage("TransferCode is required");

            // Rule for the Otp property
            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Otp is required");

            // Rule for the RiderId property
            RuleFor(x => x.RiderId)
                .NotEmpty().WithMessage("RiderId is required")
                .Must(BeValidGuid).WithMessage("RiderId should be a valid GUID format");
        }

        // Custom validation method for RiderId property
        private bool BeValidGuid(Guid riderId)
        {
            return riderId != Guid.Empty; // Check if the RiderId is not an empty GUID
        }
    }
}
