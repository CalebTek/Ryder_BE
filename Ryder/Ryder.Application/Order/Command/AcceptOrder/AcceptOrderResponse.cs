using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Command.AcceptOrder
{
    public class AcceptOrderResponse
    {

        public Guid OrderId { get; init; }
        public Guid RiderId { get; init; }
    }
}
