using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using Ryder.Application.order.Query.OrderProgress;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Import the logging library.

namespace Ryder.Application.Order.Command.EndRide
{
    public class EndRideCommand : IRequest<IResult<EndRideResponse>>
    {
        public Guid OrderId { get; set; }
        public Address DropOffLocation { get; set; }
        public Guid RiderId { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
        public string PickUpPhoneNumber { get; set; }
    }

    public class EndRideCommandValidator : AbstractValidator<EndRideCommand>
    {
        private readonly ILogger<EndRideCommandValidator> _logger; // Inject the logger.

        public EndRideCommandValidator(ILogger<EndRideCommandValidator> logger)
        {
            _logger = logger;

            // Log an information message when the validator is initialized.
            _logger.LogInformation("EndRideCommandValidator initialized.");
        }
    }
}
