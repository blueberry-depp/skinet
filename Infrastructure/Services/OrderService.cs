using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepo;

    // Basket repository continues to be unique. It's not a generic repository and it's using a different database. So it's not
    // part of our context that we're using in the unit of work.
    public OrderService(IUnitOfWork unitOfWork, 
        
       IBasketRepository basketRepo)
    {
        _unitOfWork = unitOfWork;
        _basketRepo = basketRepo;
    }   

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // The list that we need to do:
        // 1. Get basket from the repo.
        // We trust what's in the basket as in the items and the quantity of items but what we can't trust is the price has been set to,
        // and we need to check that price in database.
        // 2. Get items from the product repo.
        // 3. Get the delivery method from repo.
        // 4. Calculate subtotal. 
        // Calculate whatever the subtotal is based on what we know the prices from the items we get from the product repo.
        // 5. Create the order.
        // 6. Save to database. If success, delete the basket as well.
        // 7. Return the order.
        
        // So we're going to need some repos injected into order service, we need basket repo, product repo, delivery method repo,
        // order repo in order that we can save the order to the database.
        
        // 1. Get basket from the repo.
        var basket = await _basketRepo.GetBasketAsync(basketId);
        
        // 2. Get items from the product repo.
        var items = new List<OrderItem>();
        // Loop through what's inside basket items and we're going to get the information about each of those items from product repo.
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            // This one is kind of a snapshot of what our order is when it's been placed.
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            // We getting the price from product data in product table.
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            // We add that to items list that we created above.
            items.Add(orderItem);
        }
        
        // 3. Get the delivery method from repo.
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        
        // 4. Calculate subtotal. 
        var subtotal = items.Sum(item => item.Price * item.Quantity);
        
        // 5. Create the order.
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
        // Nothing saves the database at this point.
        _unitOfWork.Repository<Order>().Add(order);

        // 6. Save to database.
        // Because units of work owns our context. Any changes that track by entity framework are going
        // to be saved into database at this point.
        var result = await _unitOfWork.Complete();
        
        // If fail then any changes that have taken place inside method here are going to be rolled back and will send an error,
        // wo what we guarantee in this unit of work is that all of the changes in this method are going to be applied or none of are.
        // This means nothing has been saved through the database and if that's the case then we're just return null
        // and let order controller deal with sending back the error response.
        if (result <= 0) return null;
        
        // If the order is saved then we want to delete the basket as well.
        await _basketRepo.DeleteBasketAsync(basketId);

        // 7. Return the order to controller.
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        
        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }
}