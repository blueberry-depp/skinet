namespace Core.Entities.OrderAggregate;

// This is act as a snapshot of order at the time it or product item at the time it was placed
// just based on the fact that the product name might change, the product image might change
// but we wouldn't want to change it as well inside our orders so we don't want to relation
// between order and product item we're going to store the values as it was when it was ordered
// into database.
// This is not going to have an ID because it's going to be owned by the order item itself.
public class ProductItemOrdered
{
    public ProductItemOrdered()
    {
    }
    
    // We  need a constructor so that we can pass in the properties for this.
    public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
    {
        ProductItemId = productItemId;
        ProductName = productName;
        PictureUrl = pictureUrl;
    }

    public int ProductItemId { get; set; }
    public string ProductName { get; set; }
    public string PictureUrl { get; set; }
    
    
}