using Dapr.Actors.Runtime;
using Errow.Ticketing.CartApi.Interfaces;

namespace Errow.Ticketing.CartApi.Actors;

public class CartActor(ActorHost host) : Actor(host), ICartActor
{
    public Task AddToCartAsync(string seatId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromCartAsync(string seatId)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetCartAsync()
    {
        throw new NotImplementedException();
    }
}