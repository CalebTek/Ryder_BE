using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;

namespace Ryder.Application.Order.Query.GetOderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, IResult<Domain.Entities.Order>>
    {
        private readonly ApplicationContext _context;

        public GetOrderByIdQueryHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult<Domain.Entities.Order>> Handle(GetOrderByIdQuery query,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == query.OrderId, cancellationToken);
            if (order == null)
            {
                return null;
            }

            return Result<Domain.Entities.Order>.Success(order);
        }
    }
}