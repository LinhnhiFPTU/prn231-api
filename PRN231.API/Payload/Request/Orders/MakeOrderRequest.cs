namespace PRN231.API.Payload.Request.Orders;

public class MakeOrderRequest
{
    public int InventoryItemId { get; set; }
    public int Quantity { get; set; }
}