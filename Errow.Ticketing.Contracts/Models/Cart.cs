namespace Errow.Ticketing.Contracts.Models;

public class Cart
{
    public List<CartItem> Items { get; set; } = new();
    public DateTime CartDueDateTime { get; set; }
}