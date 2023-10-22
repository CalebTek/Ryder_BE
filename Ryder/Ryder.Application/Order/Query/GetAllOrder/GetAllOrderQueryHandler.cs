using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;

namespace Ryder.Application.Order.Query.GetAllOrder
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, IResult<List<Domain.Entities.Order>>>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetAllOrderQueryHandler(ApplicationContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IResult<List<Domain.Entities.Order>>> Handle(GetAllOrderQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.AppUserId.ToString());

                if(user == null)
                {
                    return Result<List<Domain.Entities.Order>>.Fail("User not found");
                }

                var allOrders = await _context.Orders.Where(o => o.AppUserId == request.AppUserId)
                    .Include(x=>x.PickUpLocation)
                    .Include(x => x.DropOffLocation)
                    .ToListAsync(cancellationToken);

                return Result<List<Domain.Entities.Order>>.Success(allOrders);
            }
            catch (Exception)
            {
                return Result<List<Domain.Entities.Order>>.Fail("Error fetching all Orders");             
            }
        }
    }
}