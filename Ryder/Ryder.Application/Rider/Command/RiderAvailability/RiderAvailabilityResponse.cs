using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Command.RiderAvailability
{
    public class RiderAvailabilityResponse
    {
        public string ValidIdUrl { get; set; }
        public string PassportPhoto { get; set; }
        public string BikeDocument { get; set; }
        public RiderAvailabilityStatus AvailabilityStatus { get; set; }
        public Guid AppUserId { get; set; }
    }
}