using Dapr.Actors.Runtime;
using Dapr.Client;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Errow.Ticketing.EventPlacementApi.Models;

namespace Errow.Ticketing.EventPlacementApi.Actors;

public class EventPlacementActor(ActorHost host, DaprClient daprClient) : Actor(host), IEventPlacementActor
{
    private readonly string _stateStoreName = "statestore";

    public async Task<EventPlacement> ReserveAsync(string seatId)
    {
        var ep = await daprClient.GetStateAsync<EventPlacement>(_stateStoreName, seatId);
        if (ep is null)
        {
            throw new ArgumentException("Seat not found");
        }
        if (!ep.Available)
        {
            throw new ArgumentException("Seat is not available");
        }
        ep.Available = false;
        await daprClient.SaveStateAsync(_stateStoreName, seatId, ep);
        
        return ep;
    }
}