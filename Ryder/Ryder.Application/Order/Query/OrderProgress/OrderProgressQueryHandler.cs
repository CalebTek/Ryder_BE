using MediatR;
using Ryder.Domain.Context;
using AspNetCoreHero.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ryder.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums.Helper;

namespace Ryder.Application.Order.Query.OrderProgress
{
    public class OrderProgressQueryHandler : IRequestHandler<OrderProgressQuery, IResult<OrderProgressResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<OrderProgressQueryHandler> _logger;

        public OrderProgressQueryHandler(ApplicationContext context, ILogger<OrderProgressQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IResult<OrderProgressResponse>> Handle(OrderProgressQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the Order by its unique identifier (orderId) and AppUserId from the context.
                var order = await _context.Orders
                    .Where(o => o.Id == request.OrderId && o.AppUserId == request.AppUserId)
                    .FirstOrDefaultAsync();

                if (order != null)
                {
                    // Log an information message when the order is found.
                    _logger.LogInformation($"Order with ID {request.OrderId} found.");

                    var response = new OrderProgressResponse
                    {
                        Status = EnumHelper.GetEnumDescription(order.Status),
                    };
                    // Return a successful result with the response data.
                    return Result<OrderProgressResponse>.Success(response);
                }

                // Log an information message when the order is not found.
                _logger.LogInformation($"Order with ID {request.OrderId} not found.");

                // Return an appropriate error result.
                return Result<OrderProgressResponse>.Fail("Order not found.");
            }
            catch (Exception ex)
            {
                // Log an error message if an exception occurs during processing.
                _logger.LogError(ex, $"Error while fetching order status for Order ID {request.OrderId}.");

                // Return an error result.
                return Result<OrderProgressResponse>.Fail("An error occurred while processing the request.");
            }
        }
    }
}
