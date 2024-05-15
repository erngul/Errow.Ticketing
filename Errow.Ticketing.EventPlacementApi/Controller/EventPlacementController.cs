using Dapr.Actors;
using Dapr.Actors.Client;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Errow.Ticketing.EventPlacementApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class EventPlacementController : ControllerBase
    {
        private readonly IEventPlacementService _eventPlacementService;
        private readonly IActorProxyFactory _actorProxyFactory;

        public EventPlacementController(IEventPlacementService eventPlacementService, IActorProxyFactory actorProxyFactory)
        {
            _eventPlacementService = eventPlacementService;
            _actorProxyFactory = actorProxyFactory;
        }

        [HttpPost("reserve/{seatId}")]
        public async Task<IActionResult> ReserveSeat(string seatId)
        {
            var actorId = new ActorId(seatId);
            var eventPlacementActor = _actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, "EventPlacementActor");

            try
            {
                var result = await eventPlacementActor.ReserveAsync(seatId);
                return Ok(new { Result = result });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("InvalidOperationException"))
                {
                    return Conflict(ex.Message); // Return 409 Seat already reserved
                }
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error for other exceptions
            }
        }

        [HttpDelete("reserve/{seatId}")]
        public async Task<IActionResult> CancelReservation(string seatId)
        {
            var actorId = new ActorId(seatId);
            var eventPlacementActor = _actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, "EventPlacementActor");
            await eventPlacementActor.CancelReservationAsync(seatId);
            return Ok();
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSeats()
        {
            var availableSeats = await _eventPlacementService.GetAvailableEventPlacementAsync();
            return Ok(availableSeats);
        }

        [HttpGet("{seatId}")]
        public async Task<IActionResult> GetSeat(string seatId)
        {
            var actorId = new ActorId(seatId);
            var eventPlacementActor = _actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, "EventPlacementActor");
            var result = await eventPlacementActor.GetEventPlacementAsync(seatId);
            return Ok(result);
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeSeats()
        {
            await _eventPlacementService.InitializeEventPlacementsAsync();
            return Ok();
        }
    }
}
