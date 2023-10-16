using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Ryder.Application.Order.Query.GetAllOrderStatus;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums.Helper;

public class GetAllOrderStatusQueryHandler : IRequestHandler<GetAllOrderStatusQuery, IResult<List<GetAllOrderStatusResponse>>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationContext _context;
    private readonly ILogger<GetAllOrderStatusQueryHandler> _logger;

    public GetAllOrderStatusQueryHandler(UserManager<AppUser> userManager, ApplicationContext context, ILogger<GetAllOrderStatusQueryHandler> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<List<GetAllOrderStatusResponse>>> Handle(GetAllOrderStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AppUserId == null)
            {
                _logger.LogError("AppUserId is null. It should be a valid identifier.");
                return Result<List<GetAllOrderStatusResponse>>.Fail("Invalid input");
            }

            var user = await _userManager.FindByIdAsync(request.AppUserId.ToString());

            if (user == null)
            {
                _logger.LogWarning($"User with ID {request.AppUserId} not found.");
                return Result<List<GetAllOrderStatusResponse>>.Fail("User not found");
            }

            var orders = _context.Orders
                .Where(order => order.AppUserId == user.Id) // Assuming there is a UserId field in the Order entity.
                .Select(order => new GetAllOrderStatusResponse
                {
                    Status = EnumHelper.GetEnumDescription(order.Status),
                    Amount = order.Amount,
                    UpdatedAt = order.UpdatedAt, 
                    OrderId = order.Id,

                })
                .ToList();

            if (orders.Any())
            {
                return Result<List<GetAllOrderStatusResponse>>.Success(orders);
            }

            _logger.LogWarning($"No orders found for the specified AppUserId: {request.AppUserId}");
            return Result<List<GetAllOrderStatusResponse>>.Fail("No orders found for the specified AppUserId.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while fetching orders for AppUserId {request.AppUserId}.");
            return Result<List<GetAllOrderStatusResponse>>.Fail("An error occurred while processing the request.");
        }
    }
}
 