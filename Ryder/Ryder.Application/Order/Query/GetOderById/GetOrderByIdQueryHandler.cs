using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;

namespace Ryder.Application.Order.Query.GetOderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, IResult<Domain.Entities.Order>>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetOrderByIdQueryHandler(ApplicationContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IResult<Domain.Entities.Order>> Handle(GetOrderByIdQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(query.AppUserId.ToString());

                if (user == null)
                {
                    return Result<Domain.Entities.Order>.Fail("User not found");
                }

                var orderId = await _context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId && o.AppUserId == query.AppUserId, cancellationToken);

                if(orderId == null)
                {
                    return Result<Domain.Entities.Order>.Fail("Order not found");
                }

                return Result<Domain.Entities.Order>.Success(orderId);
            }
            catch (Exception)
            {
                return Result<Domain.Entities.Order>.Fail("Error fetching Order Id");             
            }
         
        }
    }
}