using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryder.Application.Order.Query.GetAll;

namespace Ryder.Application.Order.Query.GetAll
{
    public class GetAllQuery : IRequest<IResult<List<GetAllQueryResponse>>>
    {
        public DateTime Created { get; set; } = new DateTime(2023, 10, 23);
    }
}