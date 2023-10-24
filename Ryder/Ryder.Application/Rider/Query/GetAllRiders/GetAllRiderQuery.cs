using AspNetCoreHero.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.GetAllRiders
{
    public class GetAllRiderQuery: IRequest<IResult<List<GetAllRiderResponse>>>
    {
    }
}
