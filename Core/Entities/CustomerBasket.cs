using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CustomerBasket
    {
        // So that we don't run into problems with redis when it comes to creating a new instance of a customer basket, we create empty constructor 
        // so that it can create a new instance without needing to know the id.
        public CustomerBasket()
        {
        }

        // Remove List<BasketItem> items in constructor because we're creating a new list
        // when we initialize or create an instance of this class but we will take the id as a parameter in here.
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        // Initialize to an empty list of items.
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
