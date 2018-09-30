using Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly DbContext _context;

        public Repository(DbContext context, DbSet<T> dbSet)
        {
            this._dbSet = dbSet;
            this._context = context;
        }

        public IQueryable<T> Get() => this._dbSet as IQueryable<T>;
        
        public T Insert(T entity)
        {
            return this._dbSet.Add(entity).Entity;
        }

        public void Update(T entity)
        {
            this._dbSet.Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }       
    }
}
