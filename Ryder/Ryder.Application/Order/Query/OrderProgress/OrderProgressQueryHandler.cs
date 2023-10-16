using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ryder.Domain.Context;
using Ryder.Domain.Enums.Helper;

namespace Ryder.Application.Order.Query.OrderProgress
{
    public class OrderProgressQueryHandler : IRequestHandler<GetAllOrderProgressQuery, IResult<OGetAllOrderrderProgressResponse>>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<OrderProgressQueryHandler> _logger;

        public OrderProgressQueryHandler(ApplicationContext context, ILogger<OrderProgressQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IResult<OGetAllOrderrderProgressResponse>> Handle(GetAllOrderProgressQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var order = await _context.Orders
                    .Where(o => o.Id == request.OrderId && o.AppUserId == request.AppUserId)
                    .FirstOrDefaultAsync();

                if (order != null)
                {

                    _logger.LogInformation($"Order with ID {request.OrderId} found.");

                    var response = new OGetAllOrderrderProgressResponse
                    {
                        Status = EnumHelper.GetEnumDescription(order.Status),


                        Amount = order.Amount,
                        UpdatedAt = DateTime.Now,

                    };

                    return Result<OGetAllOrderrderProgressResponse>.Success(response);
                }


                _logger.LogInformation($"Order with ID {request.OrderId} not found.");

                return Result<OGetAllOrderrderProgressResponse>.Fail("Order not found.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error while fetching order status for Order ID {request.OrderId}.");


                return Result<OGetAllOrderrderProgressResponse>.Fail("An error occurred while processing the request.");
            }
        }
    }
}
