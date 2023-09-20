using AspNetCoreHero.Results;
using MediatR;

namespace Ryder.Application.Rider.Query.AllRiderHistory
{
    public class RideHistoryQuery : IRequest<IResult<IList<GetOrderResponse>>>
    {
        public Guid RiderId { get; set; }
    }
}