using AspNetCoreHero.Results;
using MediatR;

namespace Ryder.Application.Order.Query.GetAllOrder
{
    public class GetAllOrderQuery : IRequest<IResult<List<Domain.Entities.Order>>>
    {
        public Guid AppUserId { get; set; }
    }
}