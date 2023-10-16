using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using AspNetCoreHero.Results;
using Ryder.Domain.Context;
using Microsoft.Extensions.Logging; 

namespace Ryder.Application.Order.Command.AcceptOrder;

public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand, IResult<string>>
{
	private readonly ApplicationContext _context;
	private readonly ILogger<AcceptOrderCommandHandler> _logger;

	public AcceptOrderCommandHandler(ApplicationContext context, ILogger<AcceptOrderCommandHandler> logger)
	{
		_context = context;
		_logger = logger;
	}
    public async Task<IResult<string>> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the order by its unique identifier (orderId) from your context
        var order = await _context.Orders.FindAsync(request.OrderId);

        if (order == null)
        {
           
            _logger.LogInformation($"Order with ID {request.OrderId} not found.");

            
            return Result<string>.Fail($"Order with ID {request.OrderId} not found.");
        }

        // Check if the rider has already declined the order
        var riderOrderStatus = _context.RequestStatuses.FirstOrDefault(ros =>
            ros.RiderId == request.RiderId && ros.OrderId == request.OrderId && ros.RiderOrderStatus == RiderOrderStatus.Declined);

        if (riderOrderStatus != null)
        {
            
            _logger.LogInformation($"Rider with ID {request.RiderId} has already declined Order with ID {request.OrderId}.");

            
            return Result<string>.Fail($"Rider with ID {request.RiderId} has already declined Order with ID {request.OrderId}.");
        }

        // Assign the rider ID to the order and update its status to "Accepted"
        order.RiderId = request.RiderId;
        order.Status = OrderStatus.InProgress;
        order.RiderOrderStatus = RiderOrderStatus.Accepted; 
        order.UpdatedAt = DateTime.UtcNow;
          
           

    // Save the updated order to your data source
    _context.Orders.Update(order);

        // Update the rider's request status
        var newRiderOrderStatus = new RequestStatus
        {
            RiderId = request.RiderId,
            OrderId = request.OrderId,
            RiderOrderStatus = RiderOrderStatus.Accepted
        };

        _context.RequestStatuses.Add(newRiderOrderStatus);

        await _context.SaveChangesAsync();

        
        _logger.LogInformation($"Order with ID {request.OrderId} accepted.");

        
        return Result<string>.Success($"Order with ID {request.OrderId} accepted.");
    }



}