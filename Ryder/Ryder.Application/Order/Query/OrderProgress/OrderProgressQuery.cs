using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Entities; 
using Ryder.Domain.Enums;
using System;
using System.Threading;

namespace Ryder.Application.Order.Query.OrderProgress
{
    public class GetAllOrderProgressQuery : IRequest<IResult<OGetAllOrderrderProgressResponse>>
    {
        public Guid OrderId { get; set; }
        public Guid AppUserId { get; set; }

    }
}
