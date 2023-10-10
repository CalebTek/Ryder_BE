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
            // Finds the rider by id
           
            var riderId = await _context.Riders.FindAsync(request.RiderId, cancellationToken);
            if (riderId == null) return (IResult<RiderEarningsResponse>)Result.Fail("Rider does not exist");

            // Get all query from the Database
            var query = from payment in _context.Payments
                        join order in _context.Orders
                        on payment.OrderId equals order.Id
                        where payment.PaymentStatus == PaymentStatus.Successful
                            && order.Status == OrderStatus.Delivered
                            && order.RiderId == riderId.Id
                        group new { payment, order } by (order.EndTime - order.StartTime).TotalHours
            into grouped
                        select new
                        {
                            TotalAmount = grouped.Sum(x => x.payment.Amount),
                            TotalRides = grouped.Count(),
                            TotalHours = TimeSpan.FromHours(grouped.Key)
                        };

            var result = query.ToList();
            var rides = _context.Orders.Where(x => x.RiderId == riderId.Id && x.Status == OrderStatus.Delivered ).OrderByDescending(i => i.CreatedAt).ToList();

            // Calculate the total earnings for successful payments
            var totalEarning = result.Sum(item => item.TotalAmount);

            // Calculate the total number of rides
            var totalRides = result.Sum(item => item.TotalRides);

            //Calculate the total ride duration
            var totalRideDuration = result.Sum(item => item.TotalHours.TotalHours);

            // An object with the calculated values
            var response = new RiderEarningsResponse
            {
                TotalEarning = totalEarning,
                TotalRides = totalRides,
                TotalRideDuration = TimeSpan.FromMinutes(totalRideDuration),
                Rides = rides
            };

            return await Result<RiderEarningsResponse>.SuccessAsync(response);

        }
    }
}
