using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ryder.Application.Rider.Command.RiderAvailability
{
    public class UpdateRiderAvailabilityCommand : IRequest<IResult<RiderAvailabilityResponse>>
    {
        [JsonIgnore] public Guid RiderId { get; set; }
        public RiderAvailabilityStatus AvailabilityStatus { get; set; }
    }
}