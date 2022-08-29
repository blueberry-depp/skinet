using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        // Empty constructor.
        public BaseSpecification()
        {
        }

        // Constructor.
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            // We set the criteria here to whatever this expression is(Expression<Func<T, bool>> criteria).
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        // Set to empty list or include list.
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        // Add the private set because we'll set what this is inside this particular class(OrderBy).
        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        // Create a method that will allow us to adds include statements to our include list which is a list of expressions.
        // "protected" means that we can access the method that we're about to create in BaseSpecification class itself and
        // any classes that derive from this class/child classes.
        // Take Expression as parameter, retun an object from this.
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            // Allow us to replace include eager loading.
            Includes.Add(includeExpression);
        }

        // These methods need to be evaluated by specification evaluators.
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        // Create methods to set the properties above(Take, Skip, IsPagingEnabled).
        protected void ApplyPaging(int skip, int take) 
        {
            Skip = skip;
            Take = take;
            // Use this property inside SpecificationEvaluator so that we know whether or not to page the results.
            IsPagingEnabled = true;
        }

    }



}
