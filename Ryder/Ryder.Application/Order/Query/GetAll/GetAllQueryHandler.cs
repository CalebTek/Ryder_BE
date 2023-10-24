using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Application.Order.Query.GetAllOrder;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Query.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IResult<List<GetAllQueryResponse>>>
    {
        private readonly ApplicationContext _context;

        public GetAllQueryHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult<List<GetAllQueryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.PickUpLocation)
                .Include(o => o.DropOffLocation)
                .Where(o => o.CreatedAt >= request.Created && o.Status == OrderStatus.OrderPlaced)
                .ToListAsync(cancellationToken);

            var orderResponses = orders.Select(order => new GetAllQueryResponse
            {
                OrderId = order.Id,
                PickUpLocationAddressDescription = order.PickUpLocation.AddressDescription,
                DropOffLocationAddressDescription = order.DropOffLocation.AddressDescription,
                PackageDescription = order.PackageDescription,
                Amount = order.Amount,
                Status = order.Status,
                Email = order.Email,
                Name = order.Name
            }).ToList();

            return Result<List<GetAllQueryResponse>>.Success(orderResponses);
        }
    }
}
