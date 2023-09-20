using Microsoft.AspNetCore.Mvc;
using Ryder.Api.Controllers;
using Ryder.Application.Rider.Command.RiderAvailability;
using Ryder.Application.Rider.Query.AllRiderHistory;
using Ryder.Application.Rider.Query.GetRiderAvailability;

public class RiderController : ApiController
{
    [HttpPost("update-availability/{id}")]
    public async Task<IActionResult> UpdateRiderAvailability(Guid id, UpdateRiderAvailabilityCommand command)
    {
        command.RiderId = id;
        return await Initiate(() => Mediator.Send(command));
    }

    [HttpGet("get-availability/{id}")]
    public async Task<IActionResult> GetRiderAvailability(Guid id)
    {
        return await Initiate(() => Mediator.Send(new GetRiderAvailabilityQuery { RiderId = id }));
    }

    [HttpGet("ride-history-by-id/{riderId}")]
    public async Task<IActionResult> GetRideHistoryById(Guid riderId)
    {
        return await Initiate(() => Mediator.Send(new RideHistoryQuery { RiderId = riderId }));
    }
}