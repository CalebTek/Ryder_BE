using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.AllRiderHistory
{
    public class RideHistoryQueryHandler : IRequestHandler<RideHistoryQuery, IResult<IList<GetOrderResponse>>>
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public RideHistoryQueryHandler(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResult<IList<GetOrderResponse>>> Handle(RideHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var rideHistory = await _context.Orders
                .Where(r => r.RiderId == request.RiderId).Include(l => l.PickUpLocation).Include(l => l.DropOffLocation).OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            if (rideHistory == null || !rideHistory.Any())
            {
                return await Result<List<GetOrderResponse>>.FailAsync();
            }

            var rideHistoryDTOs = _mapper.Map<List<GetOrderResponse>>(rideHistory);

            return Result<List<GetOrderResponse>>.Success(rideHistoryDTOs);
        }
    }
}