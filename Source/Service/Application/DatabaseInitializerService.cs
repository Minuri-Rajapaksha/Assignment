using Data.Interfaces.DbFactory.Application;
using Service.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Application
{
    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        private readonly IApplicationDbFactory _identityDbFactory;

        public DatabaseInitializerService(IApplicationDbFactory identityDbFactory)
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
