using AspNetCoreHero.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ryder.Application.Common.Hubs;
using Ryder.Domain.Context;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.GetAllRiders
{
    public class GetAllRiderHandler : IRequestHandler<GetAllRiderQuery, IResult<List<GetAllRiderResponse>>>
    {
        private readonly ApplicationContext _Context;
        public GetAllRiderHandler(ApplicationContext context)
        {
            _Context = context;
        }

         async Task<IResult<List<GetAllRiderResponse>>> IRequestHandler<GetAllRiderQuery, IResult<List<GetAllRiderResponse>>>.Handle(GetAllRiderQuery request, CancellationToken cancellationToken)
         {
            var riders = await _Context.Riders
                .Select(r => new GetAllRiderResponse
                {
                    AppUserId = r.Id,
                    AvailabilityStatus = r.AvailabilityStatus,
                    ValidIdUrl = r.ValidIdUrl,
                    PassportPhoto = r.PassportPhoto,
                    BikeDocument = r.BikeDocument
                })
                .ToListAsync(cancellationToken);

            return await Result<List<GetAllRiderResponse>>.SuccessAsync(riders);
         }
    }
}
