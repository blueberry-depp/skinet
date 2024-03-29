﻿using Core.Entities;
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
        // We also need is some additional methods to support updating.
        // T entity: as a type parameter.
        // None of these asynchronous methods. And the reason for this is that we're not
        // going to be directly adding these to the database when we use any of these methods 
        // or we saying to entity framework when we use these method is we want to add this for example. So track it,
        // and this is happening in memory, it's not happening in a sql or sqlite, etc, our repository is not responsible
        // for saving changes to the database, that's left to unit of work.
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        


    }
}
