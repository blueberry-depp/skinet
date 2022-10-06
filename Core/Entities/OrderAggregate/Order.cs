namespace Core.Entities.OrderAggregate;

// The order that's going to aggregates all of these supporting classes.
public class Order : BaseEntity
{
    public Order()
    {
    }
    
    // This is use to retrieve the list of orders for a particular user. We're not going to relate our 
    // orders to our identities which is in a separate context boundary.
    public Order(IReadOnlyList<OrderItem> orderItem, string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal)
    {
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItem;
        Subtotal = subtotal;
    }

    public string BuyerEmail { get; set; }
    // This is the local time of where the order was made on server and it includes the offsets of the time difference between UTC.
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    // This is the total of the order items and quantity added together.
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; }

    // Automapper look for get convention to run this code for total property. 
    public decimal GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}