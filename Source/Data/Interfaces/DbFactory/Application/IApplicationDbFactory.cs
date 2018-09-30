using Data.Interfaces.UnitOfWork.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.DbFactory.Application
{
    public interface IApplicationDbFactory : IDbFactoryBase<IUnitOfWorkApplication>
    {
        Task SeedDataAsync();

        Task EnsureMigrationAsync();
    }
}
