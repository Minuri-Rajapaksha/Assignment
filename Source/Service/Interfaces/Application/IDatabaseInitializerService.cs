using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface IDatabaseInitializerService
    {
        Task EnsureMigrationAsync();

        Task SeedDataAsync();
    }
}
