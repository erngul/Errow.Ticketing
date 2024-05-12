using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Errow.Ticketing.EventPlacementApi.Controller;

[ApiController]
[Route("[controller]")]
public class EventPlacementController(IEventPlacementService eventPlacementService, IActorProxyFactory actorProxyFactory) : ControllerBase
{
    [HttpPost("reserve/{seatId}")]
    public async Task<IActionResult> ReserveSeat(string seatId)
    {
        var actorId = new ActorId(seatId);
        // var actorTime = new ActorTimer("EventPlacementActor", actorId, "EventPlacementActor", "ReserveAsync", null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        var eventPlacementActor = actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, "EventPlacementActor");
        var result = await eventPlacementActor.ReserveAsync(seatId);
        return Ok(new { Result = result });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAvailableSeats()
    {
        var availableSeats = await eventPlacementService.GetAvailableEventPlacementAsync();
        return Ok(availableSeats);
    }
    
    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeSeats()
    {
        await eventPlacementService.InitializeEventPlacementsAsync();
        return Ok();
    }
}