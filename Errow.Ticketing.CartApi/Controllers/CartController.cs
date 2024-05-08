using Microsoft.AspNetCore.Mvc;

namespace Errow.Ticketing.CartApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCartStatus()
    {
        await Task.CompletedTask;
        return Ok(new { Result = "hello world" });
    }
}