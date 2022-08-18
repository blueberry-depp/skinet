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

        // Create a method that will allow us to adds include statements to our include list which is a list of expressions.
        // "protected" means that we can access the method that we're about to create in BaseSpecification class itself and
        // any classes that derive from this class/child classes.
        // Take Expression as parameter, retun an object from this.
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            // Allow us to replace include eager loading.
            Includes.Add(includeExpression);
        }

    }
}
