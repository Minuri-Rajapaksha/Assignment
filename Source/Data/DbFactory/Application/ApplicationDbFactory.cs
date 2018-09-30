using Data.Interfaces.DbFactory;
using Data.Interfaces.DbFactory.Application;
using Data.Interfaces.UnitOfWork;
using Data.Interfaces.UnitOfWork.Application;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.UnitOfWork.Application;

namespace Data.DbFactory.Application
{
    public class ApplicationDbFactory : DbFactoryBase<IUnitOfWorkApplication>, IApplicationDbFactory
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationContext CreateDbContext()
            => new ApplicationContext(new SqlConnection(this._configuration.GetConnectionString(ConnectionStrings.ApplicationConnectionString)));

        public async Task EnsureMigrationAsync()
        {
            await CreateDbContext().Database.MigrateAsync();
        }

        public async Task SeedDataAsync()
        {
            await DataSeeder.SeedAsync(this);
        }

        public override Task<IUnitOfWorkApplication> BeginUnitOfWorkAsync()
        {
            var uow = new UnitOfWorkApplication(CreateDbContext());
            var iuow = (IUnitOfWorkApplication)uow;
            return Task.FromResult((IUnitOfWorkApplication)new UnitOfWorkApplication(CreateDbContext()));
        }
    }
}
