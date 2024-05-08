using Dapr.Actors;
using Errow.Ticketing.EventPlacementApi.Models;

namespace Errow.Ticketing.EventPlacementApi.Interfaces;

public interface IEventPlacementActor : IActor
{
    Task<EventPlacement> ReserveAsync(string seatId);
}