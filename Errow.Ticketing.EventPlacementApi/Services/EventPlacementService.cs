using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using Errow.Ticketing.EventPlacementApi.Actors;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Errow.Ticketing.EventPlacementApi.Models;

namespace Errow.Ticketing.EventPlacementApi.Services;

public class EventPlacementService(DaprClient daprClient, IActorProxyFactory actorProxyFactory) : IEventPlacementService
{
    private readonly string _stateStoreName = "statestore";

    public async Task<List<EventPlacement>> GetAvailableEventPlacementAsync()
    {
        var eps = await daprClient.GetStateAsync<List<string>?>(_stateStoreName, "seats");
        if (eps is null)
        {
            return new List<EventPlacement>();
        }
        var availableEps = new List<EventPlacement>();
        foreach (var ep in eps)
        {
            var actorId = new ActorId(ep);
            var eventPlacementActor = actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, nameof(EventPlacementActor));
            availableEps.Add(await eventPlacementActor.GetStateAsync());
        }
        // var availableEps =  await daprClient.GetBulkStateAsync<EventPlacement>(_stateStoreName, ep, 0);
        var result = availableEps.OrderBy(x => x.Row).ThenBy(x => x.Column).ToList();
        return result;
    }

    public async Task InitializeEventPlacementsAsync()
    {
        await daprClient.SaveStateAsync<string>(_stateStoreName, "seats", "test");
        var epKeys = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                
                var epKey = $"seat-{i}-{j}";
                var ep = new EventPlacement(epKey, i, j, true);
                var actorId = new ActorId(epKey);
                var eventPlacementActor = actorProxyFactory.CreateActorProxy<IEventPlacementActor>(actorId, nameof(EventPlacementActor));
                await eventPlacementActor.CreateAsync(ep);
                var statestoreId = actorId.GetId();
                epKeys.Add(statestoreId);
            }
        }
        await daprClient.SaveStateAsync(_stateStoreName, "seats", epKeys);
    }
}