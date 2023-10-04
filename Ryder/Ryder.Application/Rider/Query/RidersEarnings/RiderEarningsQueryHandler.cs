using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Agreement;
using Ryder.Domain.Context;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.RidersEarnings
{
    public class RiderEarningsQueryHandler : IRequestHandler<RiderEarningsQuery, IResult<RiderEarningsResponse>>
    {
        private readonly ApplicationContext _context;
        public RiderEarningsQueryHandler(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IResult<RiderEarningsResponse>> Handle(RiderEarningsQuery request, CancellationToken cancellationToken)
        {
            // Get all payments for the rider
            var payments = from ride in _context.Riders
            where ride.Id == request.RiderId
                          join order in _context.Orders on ride.Id equals order.RiderId
                          join pay in _context.Payments on order.Id equals pay.OrderId
                          select pay;

            // Calculate the total earnings for successful payments
            var totalEarnings = payments.Where(x => x.PaymentStatus == PaymentStatus.Successful).Sum(x => x.Amount);

            // Calculate the total number of rides
            var totalRide = _context.Orders.Count(x => x.Status == OrderStatus.Delivered);

            //Calculate the total ride duration
            var totalRideDuration = TimeSpan.FromHours(_context.Orders.Where(x => x.Status == OrderStatus.Delivered).Sum(x => (x.EndTime - x.StartTime).TotalHours));

            var rides = _context.Orders.Where(x => x.Status == OrderStatus.Delivered).OrderByDescending(i => i.CreatedAt).ToList();

            // An object with the calculated values
            var response = new RiderEarningsResponse
            {
                TotalEarning = totalEarnings,
                TotalRides = totalRide,
                TotalRideDuration = totalRideDuration,
                Rides = rides
            };

            return await Result<RiderEarningsResponse>.SuccessAsync(response);
        }
    }
}
