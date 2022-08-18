using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    // Custom Value Resolver.
    // "string": destination property to be/destination return type.
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;

        // We need a constructor so that we can inject configuration.
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            // check to see if the picture is null or empty
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                // Access the ApiUrl using the property accessor([""])
                return _config["ApiUrl"] + source.PictureUrl;
            }

            // If the string is empty.
            return null;
        }
    }
}
