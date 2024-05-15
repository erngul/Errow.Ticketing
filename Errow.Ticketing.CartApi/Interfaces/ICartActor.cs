using Dapr.Actors;
using Errow.Ticketing.Contracts.Models;

namespace Errow.Ticketing.CartApi.Interfaces;

public interface ICartActor : IActor
{
    Task AddToCartAsync(string epId);
    Task RemoveFromCartAsync(string epId);
    Task<Cart> GetCartAsync();
    Task ClearCartAsync();
}