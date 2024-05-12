using Dapr.Actors;
using Errow.Ticketing.EventPlacementApi.Models;

namespace Errow.Ticketing.EventPlacementApi.Interfaces;

public interface IEventPlacementActor : IActor
{
    Task<EventPlacement> GetStateAsync();
    Task<EventPlacement> ReserveAsync(string seatId);
    Task CreateAsync(EventPlacement eventPlacement);
    // /// <summary>
    // /// Registers a timer.
    // /// </summary>
    // /// <returns>A task that represents the asynchronous save operation.</returns>
    // Task RegisterTimer();

    // Task TimerCallback(byte[] data);
}