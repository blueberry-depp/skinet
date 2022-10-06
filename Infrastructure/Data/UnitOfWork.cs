using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    // What we're going to be doing from this units of work is creating instances of the repositories and we're going to pass it.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        
        // All units of work is going to own the store context, is going to be responsible for creating UnitOfWork.
        public UnitOfWork(StoreContext context)
        {
            // We can create a new instance of store context and any repositories that we use inside this unit
            // of work are going to be stored inside Hashtable.
            _context = context;
        }

        
        // Whenever we use this method we're going to give it the type of the entity/<TEntity> it's going to check to...       
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // check if there's already a hash table created, because we've already created another instance of another repository.
            if (_repositories == null) _repositories = new Hashtable();
            
            // We want to get the name of the entity/<TEntity> and see what this actually is, let's say it's products for example.
            var type = typeof(TEntity).Name;
            
            // Check to see if hash table repositories contains a repository with this particular type.
            // type: the name of entity,              
            if (!_repositories.ContainsKey(type))
            {
                // If it doesn't we're going to create a repository type of generic repository.
                var repositoryType = typeof(GenericRepository<>);
                // Then we're going to generate or create an instance of this repository of the product for example and pass in the
                // context that we're going to get from our unit of wor.k 
                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                
                // Add this repository to the hash table.
                _repositories.Add(type, repositoryInstance);
            }
            
            // [type]: we want to query the entry and the table.
            return (IGenericRepository<TEntity>) _repositories[type];
            
            // And no matter if we create a single additional repository or a single additional entity then we don't
            // need to do anything with this unit of work because it's already set up for this, so it's very scalable
            // using the unit of work in this way to add additional repositories.
        }

        
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
