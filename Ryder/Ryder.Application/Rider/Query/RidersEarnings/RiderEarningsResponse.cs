using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.RidersEarnings
{
    public class RiderEarningsResponse
    {
        public decimal TotalEarning { get; set; }
        public int TotalRides { get; set; }
        public TimeSpan TotalRideDuration { get; set;}
        public ICollection<Ryder.Domain.Entities.Order> Rides { get; set; }
    }
}
