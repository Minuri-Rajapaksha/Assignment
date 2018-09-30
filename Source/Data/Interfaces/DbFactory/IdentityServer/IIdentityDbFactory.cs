using Data.Interfaces.UnitOfWork.IdentityServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.DbFactory.IdentityServer
{
    public interface IIdentityDbFactory : IDbFactoryBase<IUnitOfWorkIdentity>
    {
        Task SeedDataAsync();

        Task EnsureMigrationAsync();
    }
}
