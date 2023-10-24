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
using Ryder.Application.Order.Command.AcceptOrder;

namespace Ryder.Application.Order.Command.EndRide
{
    public class EndRideCommandHandler : IRequestHandler<EndRideCommand, IResult<EndRideResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<EndRideCommandHandler> _logger; // Inject the logger.
        private readonly NotificationHub _notificationHub;

		public EndRideCommandHandler(ApplicationContext context, ILogger<EndRideCommandHandler> logger, NotificationHub notificationHub)
		{
			_context = context;
			_logger = logger;
			_notificationHub = notificationHub;
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
            order.Status = OrderStatus.Delivered;
            order.EndTime = DateTime.UtcNow;

            _context.Update(order);

            await _context.SaveChangesAsync();

            // Log an information message when the order is successfully updated.
            _logger.LogInformation($"Order with ID {request.OrderId} updated successfully.");

           

            await  _notificationHub.NotifyUserAndRiderOfOrderCompleted(order.AppUserId.ToString(), order.RiderId.ToString());


			// Handle the successful update and return response
			return Result<EndRideResponse>.Success(new EndRideResponse()
            {
                OrderId = order.Id,
                RiderId = order.RiderId,
                Status= OrderStatus.Delivered,
                UpdateAt = DateTime.UtcNow,
            });
        }
    }
}
