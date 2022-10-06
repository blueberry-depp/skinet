using Core.Entities;

namespace Core.Interfaces
{
    // IDisposable: this is going to look for a disposed method in unit of work class. And when we've
    // finished our transaction is going to dispose of our context.
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;   
        // Return a number of changes to database, so entity framework is going to track all of the changes to
        // the entities where we add, where we remove, what we add things to a list,
        // whatever we do inside this unit of work we then run the complete method and that's the part that's
        // going to save our changes to database and return a number of changes.
        Task<int> Complete();
    }
}
