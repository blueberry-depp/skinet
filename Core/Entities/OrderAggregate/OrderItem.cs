namespace Core.Entities.OrderAggregate;

// This will be related to orders. We're going to have a table for order item as well.
public class OrderItem : BaseEntity
{
    public OrderItem()
    {
    }
    
    // We need a constructor so that we can populate the values in here when we construct this order item.
    public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
    {
        ItemOrdered = itemOrdered;
        Price = price;
        Quantity = quantity;
    }

    // This contains product item order that snapshots of the item we were ordering.
    public ProductItemOrdered ItemOrdered { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}