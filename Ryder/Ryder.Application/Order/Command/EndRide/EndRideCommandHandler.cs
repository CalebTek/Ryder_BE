using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;
using Ryder.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Import the logging library.
using System.Runtime.CompilerServices;
using Ryder.Application.Common.Hubs;

namespace Ryder.Application.Order.Command.EndRide
{
    public class EndRideCommandHandler : IRequestHandler<EndRideCommand, IResult<EndRideResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<EndRideCommandHandler> _logger; // Inject the logger.

        public EndRideCommandHandler(ApplicationContext context, ILogger<EndRideCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
      
            // Log an information message when the handler is initialized.
            _logger.LogInformation("EndRideCommandHandler initialized.");
        }

        public async Task<IResult<EndRideResponse>> Handle(EndRideCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the order by its unique identifier (orderId) from the context
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
            {
                // Log an information message when the order is not found.
                _logger.LogInformation($"Order with ID {request.OrderId} not found.");

                // Handle the case where the order is not found
                return Result<EndRideResponse>.Fail($"Order with ID {request.OrderId} not found.");
            }

            // Update the order details
            order.Amount = request.Amount;
            order.Status = OrderStatus.Delivered;
            order.DropOffLocation = request.DropOffLocation;
            order.PickUpPhoneNumber = request.PickUpPhoneNumber;

            // Save the updated order to your data source (e.g., database)
            _context.Update(order);

            await _context.SaveChangesAsync();

            // Log an information message when the order is successfully updated.
            _logger.LogInformation($"Order with ID {request.OrderId} updated successfully.");


			// Handle the successful update and return response
			return Result<EndRideResponse>.Success(new EndRideResponse()
            {
                OrderId = order.Id,
                RiderId = order.RiderId,
                DropOffLocation = order.DropOffLocation,
                PickUpPhoneNumber = order.PickUpPhoneNumber,
                Status = order.Status,
                Amount = order.Amount
            });
        }
    }
}
