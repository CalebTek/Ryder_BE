using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;

namespace Ryder.Application.Transfer.Command
{
    public class TransferCommand : IRequest<IResult<TransferCommandResponse>>
    {
        public string Recipient { get; set; }
        public int Amount { get; set; }
        public string ReasonForTransfer { get; set; }
        public string Currency { get; set; } = "NGN";
        public Guid RiderId { get; set; }
    }

    public class TransferCommandValidator : AbstractValidator<TransferCommand>
    {
        public TransferCommandValidator()
        {
            // Rule for the Recipient property (bank account number)
            RuleFor(x => x.Recipient)
                .NotEmpty().WithMessage("Recipient is required")
                .Length(10).WithMessage("Recipient should be exactly 10 characters");

            // Rule for the Amount property
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount should be greater than 0");

            // Rule for the ReasonForTransfer property
            RuleFor(x => x.ReasonForTransfer)
                .NotEmpty().WithMessage("Reason for transfer is required")
                .MaximumLength(30).WithMessage("Reason for transfer should be less than 30 characters");

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
