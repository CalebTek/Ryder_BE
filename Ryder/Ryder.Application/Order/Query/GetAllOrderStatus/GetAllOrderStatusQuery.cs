using AspNetCoreHero.Results;
using MediatR;
using Ryder.Application.Order.Query.OrderProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Query.GetAllOrderStatus
{
    public class GetAllOrderStatusQuery : IRequest<IResult<List<GetAllOrderStatusResponse>>>
    {
        public Guid AppUserId { get; set; }
    }
}
