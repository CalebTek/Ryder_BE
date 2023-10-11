using AspNetCoreHero.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Ryder.Application.Order.Command.AcceptOrder;
using Ryder.Domain.Enums;
using Ryder.Application.Order.Command.DeclineOrder;

namespace Ryder.Domain.Context
{
    public class DeclineOrderCommand : IRequest<IResult<string>>
    {
        public Guid OrderId { get; init; }
        public Guid RiderId { get; init; }

       
    }
}



