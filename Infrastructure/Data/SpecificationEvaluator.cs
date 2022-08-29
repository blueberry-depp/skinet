using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // BaseEntity: We constraining this only for use with our actual entity classes/BaseEntity.
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        // TEntity: which is DB set of type product and because it's a DB set we can now use the data context 
        // or the DB context methods to actually apply the queries into our expression,
        // in this case we're going to check to see if the criteria is not equal to null(spec.Criteria != null),
        // it won't be because we know we've got the id And in this case we're going to hit this method(query.Where(spec.Criteria)) and we're
        // going to add a "where" query to our query. So we're going to say where the id is equal to the product id and we're also hit the method
        // (spec.Includes.Aggregate(query, (current, include) => current.Include(include))) and this is going to aggregates are include expressions,
        // so it's going to include the product type and the product brand and then we're going to return the query.
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            // Store input query.
            var query = inputQuery;

            // Evaluate what's inside this specification(spec)/check to see if we have an Criteria in our specification.
            if (spec.Criteria != null)
            {
                // Get the product for instance where the product is whatever we've specified as this criteria/(spec.Criteria)
                // this could be for example the where clause in here could be our expression and remember our expressions
                // are just going to be lambda expressions(p => p.ProductTypeId == id) for example.
                query = query.Where(spec.Criteria);
            }

            // After this all set up, then capture this information from the client and we can do that in the query string of the http requests.
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Check to see if we want to apply paging.
            if (spec.IsPagingEnabled)
            {
                // Note: The ordering of this is important because if we're filtering results early on then we wouldn't want
                // to page our results before we know what results we're returning. So the paging operators need to come after any filtering operators.
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Evaluate the includes.
            // "Aggregate" because we're combining all of our include operations.
            // "current" is entity.
            // "include" is the expression of our includes statement.
            // This method doing is taken out two include statements or it will do and then aggregate and pass them into our query which
            // is going to be an IQueryable that we then pass to our list or pass to a method so
            // then it can query the database and return a results based on what's contained in this.
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
