using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // base(): we can also use it for is to decide whether or not to send back the products of a specific brand or a specific type and
        // we use criteria for filter because criteria is where clause.
        public ProductsWithTypesAndBrandsSpecification(ProductParamsSpecification productParams)
           
            : base(x =>
                // Check to see if the string has a value, we're comparing like for like and use the Contains to see if it contains the search term.
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                // Check to see brandId have value, if not have value, then we'll create a query get all of the products that match the brandId
                // that we're passing paramater or execute the right condition.
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && 
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            // Add some conditions to check the value of the sort and then we'll apply the correct ordering accordingly, 
            // use a switch statement to see what the value of the sort and then apply the appropriate ordering to the ordering to the appropriate property.
            // Checked to see if the string is null or empty. 
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        // base(criteria): is what we gonna change in BaseSpecification with paramater.
        // We create a new instance of this and we've built up our expressions we've replaced the generics with actual expressions.
        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
