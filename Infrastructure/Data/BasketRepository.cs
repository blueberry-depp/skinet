using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Data
{
    // Get redis database so that we can work with this directly. No need to go through entity framework to abstract
    // ourselves away from this. We're just going to be able to get add and remove things from this. It's just
    // nosql database at key and value store living inside memory on our server.
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
       
        public BasketRepository(IConnectionMultiplexer redis)
        {
            // Now we've got a connection to database available for us to use so that we can do whatever we need to
            // do to add a basket remove our basket etc.
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            // Baskets are going to be stored as strings in redis database.
            var data = await _database.StringGetAsync(basketId);

            // What we're going to do is take our object json that comes up from the client and we're going
            // to serialize that into a string which is stored in redis database as a string and when we want
            // to get it out we're going to deserialize it back into something we can use or deserialize it into customer basket,
            // so if we have data then we going deserialize that into customer basket.
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        // This for create or update the basket,
       
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            // If we're updating baskets what we're going to do is simply replace the existing basket in redis database and 
            // just replace it with whatever is coming up from the client as the new basket. So we're nott update individual
            // values in here we're simply going to replace the existing string that represents our database with the new version of this basket.
            var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created) return null;

            // We'll just make use of this particular method because we've got Json serializer.deserialize in GetBasketAsync methos.
            return await GetBasketAsync(basket.Id);

        }
    }
}
