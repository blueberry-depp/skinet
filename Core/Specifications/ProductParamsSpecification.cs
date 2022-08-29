using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductParamsSpecification
    {
        private const int MaxPageSize = 50;
        // This is auto properties. And this comes with a getter and a setter.
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        // This is more specific about what these actually do and to get method is going to return page size and
        // and the set methods in here what we want to do is set the page size.
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        private string _search;
        public string? Search
        {
            get => _search;
            // We always want it to match against something lowercase.
            set => _search = value.ToLower();
        }
    }
}
