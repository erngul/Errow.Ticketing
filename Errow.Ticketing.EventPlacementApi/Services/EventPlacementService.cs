using Dapr.Client;
using Errow.Ticketing.Contracts.Models;
using Errow.Ticketing.EventPlacementApi.Interfaces;

namespace Errow.Ticketing.EventPlacementApi.Services;

public class EventPlacementService(DaprClient daprClient) : IEventPlacementService
{
    private readonly string _stateStoreName = "statestore";

    public async Task<List<EventPlacement>> GetAvailableEventPlacementAsync()
    {
        var ep = await daprClient.GetStateAsync<List<string>?>(_stateStoreName, "seats");
        if (ep is null)
        {
            return new List<EventPlacement>();
        }
        var availableEps =  await daprClient.GetBulkStateAsync<EventPlacement>(_stateStoreName, ep, 0);
        var result = availableEps.Select(x => x.Value).OrderBy(x => x.Row).ThenBy(x => x.Column).ToList();
        return result;
    }

    public async Task InitializeEventPlacementsAsync()
    {
        var epKeys = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                var epKey = $"seat-{i}-{j}";
                var ep = new EventPlacement(epKey, i, j, true);
                await daprClient.SaveStateAsync(_stateStoreName, epKey, ep);
                epKeys.Add(epKey);
            }
        }
        await daprClient.SaveStateAsync(_stateStoreName, "seats", epKeys);
    }
}