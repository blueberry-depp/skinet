using API.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _config;

    public OrderItemUrlResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        // check to see if the picture is null or empty
        if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
        {
            // Access the ApiUrl using the property accessor([""])
            return _config["ApiUrl"] + source.ItemOrdered.PictureUrl;
        }

        // If the string is empty.
        return null;
    }
}