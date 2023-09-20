using AspNetCoreHero.Results;
using MediatR;

namespace Ryder.Application.Order.Query.GetOderById
{
    public class GetOrderByIdQuery : IRequest<IResult<Domain.Entities.Order>>
    {
        public Guid OrderId { get; set; }
    }
}