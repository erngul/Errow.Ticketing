using Dapr.Actors.Runtime;
using Dapr.Client;
using Errow.Ticketing.CartApi.Interfaces;

namespace Errow.Ticketing.CartApi.Actors;

public class CartActor(ActorHost host) : Actor(host), ICartActor, IRemindable
{
    public async Task AddToCartAsync(string seatId)
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();
        var url = $"http://eventplacementapi/eventplacement/reserve/{seatId}";
        var responseMessage = await httpClient.PostAsync(url, null);
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to reserve seat {seatId}");
        }
        
        var state = await StateManager.GetOrAddStateAsync("cart", new List<string>());
        state.Add(seatId);
        await StateManager.AddOrUpdateStateAsync("cart", state, (s, value) => state);

    }

    public async Task RemoveFromCartAsync(string seatId)
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();
        var url = $"http://eventplacementapi/eventplacement/reserve/{seatId}";
        var responseMessage = await httpClient.DeleteAsync(url);
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to cancel reservation for seat {seatId}");
        }
        
        var state = await StateManager.TryGetStateAsync<List<string>>("cart");
        if (state.HasValue)
        {
            state.Value.Remove(seatId);
            await StateManager.AddOrUpdateStateAsync("cart", state.Value, (s, value) => state.Value);
        }
    }

    public async Task<List<string>> GetCartAsync()
    {
        var result = await StateManager.TryGetStateAsync<List<string>>("cart");
        return result.HasValue ? result.Value : new List<string>();
    }

    public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        throw new NotImplementedException();
    }
}