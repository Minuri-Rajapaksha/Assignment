
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        T Insert(T entity);        

        void Update(T entity);
    }
}
