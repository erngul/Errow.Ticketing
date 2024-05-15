using Dapr.Actors;
using Errow.Ticketing.Contracts.Models;

namespace Errow.Ticketing.EventPlacementApi.Interfaces;

public interface IEventPlacementActor : IActor
{
    Task<EventPlacement> ReserveAsync(string seatId);
    Task CancelReservationAsync(string seatId);

    Task<EventPlacement> GetEventPlacementAsync(string seatId);
    // /// <summary>
    // /// Registers a timer.
    // /// </summary>
    // /// <returns>A task that represents the asynchronous save operation.</returns>
    // Task RegisterTimer();

    // Task TimerCallback(byte[] data);
}