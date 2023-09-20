using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.GetRiderAvailability
{
    public class GetRiderAvailabilityQuery : IRequest<IResult<GetRiderAvailabilityResponse>>
    {
        public Guid RiderId { get; set; }

        public RiderAvailabilityStatus AvailabilityStatus { get; set; }
    }
}