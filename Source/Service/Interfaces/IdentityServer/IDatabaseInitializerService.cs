using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IdentityServer
{
    public interface IDatabaseInitializerService
    {
        Task EnsureMigrationAsync();

        Task SeedDataAsync();
    }
}
