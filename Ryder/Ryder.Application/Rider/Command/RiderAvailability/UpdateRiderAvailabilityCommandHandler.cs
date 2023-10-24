using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Ryder.Application.Common.Hubs;
using Ryder.Domain.Context;
using Ryder.Domain.Enums;

namespace Ryder.Application.Rider.Command.RiderAvailability
{
    public class
        UpdateRiderAvailabilityCommandHandler : IRequestHandler<UpdateRiderAvailabilityCommand,
            IResult<RiderAvailabilityResponse>>
    {
        private readonly ApplicationContext _Context;

		public UpdateRiderAvailabilityCommandHandler(ApplicationContext context)
		{
			_Context = context;
			
		}

		public async Task<IResult<RiderAvailabilityResponse>> Handle(UpdateRiderAvailabilityCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = new RiderAvailabilityResponse
                {
                    AvailabilityStatus = request.AvailabilityStatus,
                    AppUserId = request.RiderId
                };

                // Retrieve the rider entity from the database using the RiderId
                var rider = await _Context.Riders.FindAsync(request.RiderId);

                if (rider == null)
                {
                    // Rider with the given ID doesn't exist
                    return await Result<RiderAvailabilityResponse>.FailAsync("Rider not Found");
                }

                // Update the availability status
                rider.AvailabilityStatus = response.AvailabilityStatus;
                _Context.Entry(rider).State = EntityState.Modified;

                // Save changes to the database
                await _Context.SaveChangesAsync();

				// Create and return the response

				return Result<RiderAvailabilityResponse>.Success(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                return await Result<RiderAvailabilityResponse>.FailAsync($"An error occurred: {ex.Message}");
            }
        }
    }
}