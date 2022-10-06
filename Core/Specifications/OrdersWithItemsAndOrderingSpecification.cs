using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications;

// Return the Order.
public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
{
    // Use the base constructor to specify the criteria: all of orders where the order.BuyerEmail is equal to email.
    public OrdersWithItemsAndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
    {            
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderDate);
    }
    
    public OrdersWithItemsAndOrderingSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}