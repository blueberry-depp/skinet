using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Get something by some sort of criteria.
        // Expression takes a function and the function takes a type and what it returning. In this case is going to be a boolean value,
        // the criteria is the thing that we're going to get, is WHERE the product has a type of one or type id of one for instance.
        Expression<Func<T, bool>> Criteria { get; }
        // This is for include operation.
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        // Take a certain amount of records or certain amount of products.
        int Take { get; }
        // Skip a certain amount of records or certain amount of products.
        int Skip { get; }
        bool IsPagingEnabled { get; }


    }
}
