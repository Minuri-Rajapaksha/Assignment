using Data.Interfaces.DbFactory.IdentityServer;
using Data.Interfaces.UnitOfWork.IdentityServer;
using Data.UnitOfWork.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Data.DbFactory.IdentityServer
{
    public class IdentityDbFactory : DbFactoryBase<IUnitOfWorkIdentity>, IIdentityDbFactory
    {
        private readonly IConfiguration _configuration;

        public IdentityDbFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IdentityContext CreateDbContext()
            => new IdentityContext(new SqlConnection(this._configuration.GetConnectionString("IdentityConnection")));

        public override Task<IUnitOfWorkIdentity> BeginUnitOfWorkAsync()
        {
            return Task.FromResult((IUnitOfWorkIdentity)(new UnitOfWorkIdentity(CreateDbContext())));
        }

        public async Task EnsureMigrationAsync()
        {
            await CreateDbContext().Database.MigrateAsync();
        }

        public async Task SeedDataAsync()
        {
            await DataSeeder.SeedAsync(this);
        }
    }
}
