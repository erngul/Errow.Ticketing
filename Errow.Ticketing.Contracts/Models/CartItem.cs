namespace Errow.Ticketing.Contracts.Models;
public class CartItem
{
    public string EventPlacementId { get; set; } = string.Empty;
    public DateTime CreatedDateTime { get; set; } = DateTime.MinValue;
    public DateTime? DueDateTime { get; set; }
}