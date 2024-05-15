using Dapr.Actors;
using Dapr.Actors.Client;
using Errow.Ticketing.CartApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Errow.Ticketing.CartApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IActorProxyFactory _actorProxyFactory;

        public CartController(IActorProxyFactory actorProxyFactory)
        {
            _actorProxyFactory = actorProxyFactory ?? throw new ArgumentNullException(nameof(actorProxyFactory));
        }

        [HttpGet]
        public async Task<IActionResult> GetCartStatus()
        {
            var actorId = new ActorId("cart");
            var cartActor = _actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
            var result = await cartActor.GetCartAsync();
            return Ok(new { Result = result });
        }

        [HttpPost("add/{seatId}")]
        public async Task<IActionResult> AddToCart(string seatId)
        {
            if (string.IsNullOrEmpty(seatId))
            {
                return BadRequest("Seat ID cannot be null or empty.");
            }

            var actorId = new ActorId("cart");
            var cartActor = _actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");

            try
            {
                await cartActor.AddToCartAsync(seatId);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("InvalidOperationException"))
                {
                    return Conflict(ex.Message); // Return 409 Seat already reserved
                }
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error for other exceptions
            }
        }

        [HttpPost("remove/{seatId}")]
        public async Task<IActionResult> RemoveFromCart(string seatId)
        {
            if (string.IsNullOrEmpty(seatId))
            {
                return BadRequest("Seat ID cannot be null or empty.");
            }

            var actorId = new ActorId("cart");
            var cartActor = _actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
            await cartActor.RemoveFromCartAsync(seatId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var actorId = new ActorId("cart");
            var cartActor = _actorProxyFactory.CreateActorProxy<ICartActor>(actorId, "CartActor");
            await cartActor.ClearCartAsync();
            return Ok();
        }
    }
}
