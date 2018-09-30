using Data.Interfaces.DbFactory.IdentityServer;
using Service.Interfaces.IdentityServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IdentityServer
{
    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        private readonly IIdentityDbFactory _identityDbFactory;

        public DatabaseInitializerService(IIdentityDbFactory identityDbFactory)
        {
            _identityDbFactory = identityDbFactory;
        }

        public async Task EnsureMigrationAsync()
        {
            await _identityDbFactory.EnsureMigrationAsync();
        }

        public async Task SeedDataAsync()
        {
            await _identityDbFactory.SeedDataAsync();
        }
    }
}
