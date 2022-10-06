using Core.Entities.OrderAggregate;

namespace API.Dtos;

// Shaping the return data. 
public class OrderToReturnDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    // This is the local time of where the order was made on server and it includes the offsets of the time difference between UTC.
    public DateTimeOffset OrderDate { get; set; }
    public Address ShipToAddress { get; set; }
    public string DeliveryMethod { get; set; }
    public decimal ShippingPrice { get; set; }
    public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
    // This is the total of the order items and quantity added together.
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
}