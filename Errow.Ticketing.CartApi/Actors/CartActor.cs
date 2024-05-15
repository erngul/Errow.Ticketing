using Dapr.Actors.Runtime;
using Dapr.Client;
using Errow.Ticketing.CartApi.Interfaces;
using Errow.Ticketing.Contracts.Models;
using Newtonsoft.Json;

namespace Errow.Ticketing.CartApi.Actors;

public class CartActor(ActorHost host) : Actor(host), ICartActor, IRemindable
{
    public async Task AddToCartAsync(string epId)
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();
        var url = $"http://eventplacementapi/eventplacement/reserve/{epId}";
        var responseMessage = await httpClient.PostAsync(url, null);

        if (!responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new InvalidOperationException($"Seat {epId} is already reserved");
            }

            throw new Exception($"Failed to reserve seat {epId}");
        }

        var cartItem = new CartItem()
        {
            EventPlacementId = epId,
            CreatedDateTime = DateTime.Now
        };
        await RegisterReminder(epId);

        var state = await StateManager.GetOrAddStateAsync("cart", new List<CartItem>());
        state.Add(cartItem);
        await StateManager.AddOrUpdateStateAsync("cart", state, (s, value) => state);
    }

    public async Task RemoveFromCartAsync(string epId)
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();
        var url = $"http://eventplacementapi/eventplacement/reserve/{epId}";
        var responseMessage = await httpClient.DeleteAsync(url);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to cancel reservation for seat {epId}");
        }

        var state = await StateManager.TryGetStateAsync<List<CartItem>>("cart");
        if (state.HasValue)
        {
            state.Value.Remove(state.Value.First(x => x.EventPlacementId == epId));
            await StateManager.AddOrUpdateStateAsync("cart", state.Value, (s, value) => state.Value);
        }

        await UnregisterReminderAsync(epId);
    }

    public async Task<Cart> GetCartAsync()
    {
        var items = await StateManager.TryGetStateAsync<List<CartItem>>("cart");
        if (!items.HasValue)
        {
            return new Cart();
        }

        var cart = new Cart();
        foreach (var item in items.Value)
        {
            var reminder =
                await Host.TimerManager.GetReminderAsync(new ActorReminderToken(Host.ActorTypeInfo.ActorTypeName, Id,
                    item.EventPlacementId));
            item.DueDateTime = item.CreatedDateTime.Add(reminder.DueTime);
        }

        cart.Items = items.Value;
        return cart;
    }

    public async Task ClearCartAsync()
    {
        await StateManager.TryRemoveStateAsync("cart");
    }

    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        await RemoveFromCartAsync(reminderName);
    }

    public async Task RegisterReminder(string epId)
    {
        await RegisterReminderAsync(epId, null, TimeSpan.FromMinutes(5), TimeSpan.FromMilliseconds(-1));
    }
}