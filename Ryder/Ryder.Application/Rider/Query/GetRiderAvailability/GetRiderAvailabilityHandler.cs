using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Ryder.Application.Common.Hubs;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;

namespace Ryder.Application.Rider.Query.GetRiderAvailability
{
    public class GetRiderAvailabilityHandler : IRequestHandler<GetRiderAvailabilityQuery, IResult<GetRiderAvailabilityResponse>>
    {
        private readonly ApplicationContext _Context;
       
		public GetRiderAvailabilityHandler(ApplicationContext context)
		{
			_Context = context;
			
		}

		public async Task<IResult<GetRiderAvailabilityResponse>> Handle(GetRiderAvailabilityQuery request,
            CancellationToken cancellationToken)
        {
            var rider = await _Context.Riders
                .Where(r => r.Id == request.RiderId)
                .FirstOrDefaultAsync(cancellationToken);

            if (rider == null)
            {
                return await Result<GetRiderAvailabilityResponse>.FailAsync("Rider not found");
            }

            var response = new GetRiderAvailabilityResponse
            {
                AppUserId = rider.Id,
                AvailabilityStatus = rider.AvailabilityStatus
            };

			return await Result<GetRiderAvailabilityResponse>.SuccessAsync(response);
        }
    }
}