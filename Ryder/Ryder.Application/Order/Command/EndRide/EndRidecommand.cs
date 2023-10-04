using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
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
        public Guid RiderId { get; init; }
        public Guid OrderId { get; init; }
        
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
