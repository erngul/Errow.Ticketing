using Dapr.Actors;

namespace Errow.Ticketing.CartApi.Interfaces;

public interface ICartActor : IActor
{
    Task AddToCartAsync(string seatId);
    Task RemoveFromCartAsync(string seatId);
    Task<List<string>> GetCartAsync();
}