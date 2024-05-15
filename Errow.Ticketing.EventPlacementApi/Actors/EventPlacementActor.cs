using Dapr.Actors.Runtime;
using Dapr.Client;
using Errow.Ticketing.Contracts.Models;
using Errow.Ticketing.EventPlacementApi.Interfaces;

namespace Errow.Ticketing.EventPlacementApi.Actors;

public class EventPlacementActor(ActorHost host, DaprClient daprClient) : Actor(host), IEventPlacementActor
{
    private EventPlacement _ep = new EventPlacement();
    private readonly string _stateStoreName = "statestore";

    public async Task<EventPlacement> ReserveAsync(string seatId)
    {
        _ep = await daprClient.GetStateAsync<EventPlacement>(_stateStoreName, host.Id.ToString());

        if (_ep is null)
        {
            throw new ArgumentException("Seat not found");
        }

        if (!_ep.Available)
        {
            throw new InvalidOperationException("Seat is already reserved");
        }

        _ep.Available = false;
        await daprClient.SaveStateAsync(_stateStoreName, host.Id.ToString(), _ep);

        return _ep;
    }

    public async Task CancelReservationAsync(string seatId)
    {
        _ep = await daprClient.GetStateAsync<EventPlacement>(_stateStoreName, host.Id.ToString());

        if (_ep is null)
        {
            throw new ArgumentException("Seat not found");
        }

        _ep.Available = true;
        await daprClient.SaveStateAsync(_stateStoreName, host.Id.ToString(), _ep);
    }

    public async Task<EventPlacement> GetEventPlacementAsync(string seatId)
    {
        _ep = await daprClient.GetStateAsync<EventPlacement>(_stateStoreName, host.Id.ToString());

        if (_ep is null)
        {
            throw new ArgumentException("Seat not found");
        }

        return _ep;
    }
}