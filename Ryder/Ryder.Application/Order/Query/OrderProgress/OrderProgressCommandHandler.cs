using AspNetCoreHero.Results;
using MediatR;
//using Ryder.Application.order.Query.AcceptOrder;
using Ryder.Domain.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Import the logging library.

namespace Ryder.Application.order.Query.OrderProgress
{
    public class OrderProgressCommandHandler : IRequestHandler<OrderProgressCommand, IResult<OrderProgressResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<OrderProgressCommandHandler> _logger; // Inject the logger.

        public OrderProgressCommandHandler(ApplicationContext context, ILogger<OrderProgressCommandHandler> logger)
        {
            _context = context;
            _logger = logger;

            // Log an information message when the handler is initialized.
            _logger.LogInformation("OrderProgressCommandHandler initialized.");
        }

        public async Task<IResult<OrderProgressResponse>> Handle(OrderProgressCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the order by its unique identifier (orderId)
            var order = await _context.Orders.FindAsync(request.Status);

            if (order == null)
            {
                // Log an information message when the order is not found.
                _logger.LogInformation($"Order with ID {request.Status} not found.");

                // Handle the case where the order is not found
                return Result<OrderProgressResponse>.Fail($"Order with ID {request.Status} not found.");
            }

            // Update the order's progress
            order.Status = request.Status;
            order.PickUpLocation = request.PickUpLocation;
            order.DropOffLocation = request.DropOffLocation;

            try
            {
                // Save the updated order to your data source (e.g., database)
                await _context.SaveChangesAsync();

                // Log an information message when the order is successfully updated.
                _logger.LogInformation($"Order with ID {request.Status} updated.");

                // Handle the successful update
                return Result<OrderProgressResponse>.Success($"Order with ID {request.Status} updated.");
            }
            catch (Exception ex)
            {
                // Log an information message when an exception occurs during the update.
                _logger.LogInformation($"Error updating order with ID {request.Status}: {ex.Message}");

                // Handle any exceptions that may occur during the update
                return Result<OrderProgressResponse>.Fail($"Error updating order with ID {request.Status}: {ex.Message}");
            }
        }
    }
}
