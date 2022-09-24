using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // Create a specific repository for baskets. We can't use our generic repository that created earlier on
    // because that one was very specific for entity framework and we're not using entity framework for basket. We're
    // using redis instead. And we're going to interact directly with the redis database from our repository. And we're not going to use
    // entity framework to send our queries to and from redis.

    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        // Return CustomerBasket(<CustomerBasket>) and take instance of CustomerBasket(CustomerBasket basket).
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
