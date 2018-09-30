
using Data.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Data.DbFactory
{
    public abstract class DbFactoryBase<TUnitOfWork>
        where TUnitOfWork : class, IUnitOfWorkBase
    {

        public abstract Task<TUnitOfWork> BeginUnitOfWorkAsync();

    }
}
