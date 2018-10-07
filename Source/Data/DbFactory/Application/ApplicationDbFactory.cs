using Data.Interfaces.DbFactory.Application;
using Data.Interfaces.UnitOfWork.Application;
using Data.UnitOfWork.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data.DbFactory.Application
{
    public class ApplicationDbFactory : DbFactoryBase<IUnitOfWorkApplication>, IApplicationDbFactory
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual ApplicationContext CreateDbContext()
            => new ApplicationContext(new SqlConnection(this._configuration.GetConnectionString("ApplicationConnection")));

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
            return Task.FromResult((IUnitOfWorkApplication)new UnitOfWorkApplication(CreateDbContext()));
        }
    }
}
