namespace Core.Entities.OrderAggregate;

// We don't need a constructor because users are just going to select a delivery method and we'll use the
// delivery method ID to decide which delivery method this is gonna be in order, so that means there's going to be
// a table for delivery methods.
public class DeliveryMethod : BaseEntity
{
    public string ShortName { get; set; }
    public string DeliveryTime { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}