using Core.Entities.OrderAggregate;

namespace Core.Interfaces;

// We're going to do this inside order service that we're going to create and the purpose of this is to keep the
// logic for this outside of controller logic. We don't want controllers to become too big basically and the order process is going to require.
// the use of a few different repositories and we'll also need to do some calculations as well.
public interface IOrderService
{
    Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
    Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
    // Just for convenience because we're already going to be bringing repositories into order service and if we
    // don't do this here we'll have to do it in controller. And that mean also adding to our controller the
    // repository as well and we can avoid that just by shifting this functionality into order service.
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
}