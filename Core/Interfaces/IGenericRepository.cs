using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // We only want this to be usable by classes that derive from base entity. And this is another reason for
    // creating that base entity as we did earlier on. This means that only our entities can be used
    // with our generic repository if we attempted to use our products controller with this then we would get an error,
    // we also need to add this to service container so that we can inject this wherever we need it in our application.
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

    }
}
