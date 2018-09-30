using Data.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.DbFactory
{
    public interface IDbFactoryBase<TUnitOfWork> where TUnitOfWork : class, IUnitOfWorkBase
    {
        Task<TUnitOfWork> BeginUnitOfWorkAsync();
    }
}
