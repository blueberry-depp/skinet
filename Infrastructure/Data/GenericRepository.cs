﻿using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // Set: set T which is going to be whatever it is we want to get.
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();

        }

        // "spec" contains where clause what we want to get with the ID as well as the include expressions 
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            // FirstOrDefaultAsync(): pass a IQueryable to a method that's going to execute the query on database,
            // return the first product that matches what's inside the specification(spec), In other words what is the product
            // that matches the id that we're passing and also include the types and the brands of the products.
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            // ToListAsync(): because we want to return a list of things.
            return await ApplySpecification(spec).ToListAsync();
        }


        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            // Count the results.
            return await ApplySpecification(spec).CountAsync();
        }


        // We're going to hit GetQuery method in the SpecificationEvaluator and this job of this method is
        // to return us and IQueryable and we're passing the DB sets(_context.Set<T>()) which is gonna to be the product DB sets
        // and we're also passing the specification(spec) to SpecificationEvaluator.
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            // Set: set the type of entity.
            // Returning specification which is now IQueryable.
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

     
    }
}
