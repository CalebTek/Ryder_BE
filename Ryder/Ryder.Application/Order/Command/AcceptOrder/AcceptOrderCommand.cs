using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using Ryder.Application.User.Query.GetCurrentUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Import the logging library.
using Ryder.Domain.Enums;

namespace Ryder.Application.Order.Command.AcceptOrder
{
    public class AcceptOrderCommand : IRequest<IResult<string>>
    {
        public Guid OrderId { get; init; }
        public Guid RiderId { get; init; }
      
    }

    public class AcceptOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
    {
        private readonly ILogger<AcceptOrderCommandValidator> _logger; // Inject the logger.

        public AcceptOrderCommandValidator(ILogger<AcceptOrderCommandValidator> logger)
        {
            _logger = logger;

            // Log an information message when the validator is initialized.
            _logger.LogInformation("AcceptOrderCommandValidator initialized.");
        }
    }
}
