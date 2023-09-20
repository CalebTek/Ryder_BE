using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;

namespace Ryder.Application.Order.Query.GetAllOrder
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, IResult<List<Domain.Entities.Order>>>
    {
        private readonly ApplicationContext _context;

        public GetAllOrderQueryHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult<List<Domain.Entities.Order>>> Handle(GetAllOrderQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.ToListAsync(cancellationToken);
            return Result<List<Domain.Entities.Order>>.Success(orders);
        }
    }
}