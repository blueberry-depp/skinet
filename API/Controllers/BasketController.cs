using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // we're not storing anything in actual database here. It's just a place where customers can
    // leave their baskets behind in our memory. So if they come back, they can pick up where they left off, and what will
    // store on the client side is going to be the basket's ID and will use that from the client to go and get whatever
    // basket that left inside the memory, and if they leave a basket in memory or in redis for a month and they don't come back to it, then
    // it's going to be destroyed anyway. And if that was the case, they just have to go and put the items back in another virtual basket.

    // BaseApiController: which gives us route, so our route is going to be api/basket to hit any of the endpoints inside here.
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        // We give constructor so that we can inject basket repository.
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        // Return ActionResult that contain CustomerBasket.
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            // It could be null, and if it is, then what we'll return a new customer basket and we'll use the id that the client has generated to give
            // them back a new basket with an empty list of items.
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            // Send them back updated basket.  
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

        }






    }
}
