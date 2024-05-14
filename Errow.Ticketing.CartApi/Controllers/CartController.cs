using Dapr.Actors;
using Dapr.Actors.Client;
using Errow.Ticketing.CartApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Errow.Ticketing.CartApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(IActorProxyFactory actorProxyFactory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCartStatus()
    {
        var actorId = new ActorId("cart");
        var cartActor = actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
        var result = await cartActor.GetCartAsync();
        return Ok(new { Result = result });
    }
    
    [HttpPost("add/{seatId}")]
    public async Task<IActionResult> AddToCart(string seatId)
    {
        var actorId = new ActorId("cart");
        var cartActor = actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
        await cartActor.AddToCartAsync(seatId);
        return Ok();
    }
    
    [HttpPost("remove/{seatId}")]
    public async Task<IActionResult> RemoveFromCart(string seatId)
    {
        var actorId = new ActorId("cart");
        var cartActor = actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
        await cartActor.RemoveFromCartAsync(seatId);
        return Ok();
    }
    
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var actorId = new ActorId("cart");
        var cartActor = actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
        var cart = await cartActor.GetCartAsync();
        foreach (var seatId in cart)
        {
            await cartActor.RemoveFromCartAsync(seatId);
        }
        return Ok();
    }
}