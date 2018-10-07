using Data.DbFactory.Application;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service.Tests.Interfaces;
using Shared.Constants;

namespace Service.Tests
{
    public class ApplicationDbFactoryTest : ApplicationDbFactory
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbFactoryTest(IConfiguration configAccessor) : base(configAccessor)
        {
            _configuration = configAccessor;
        }

        public override ApplicationContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("ApplicationTest-" + _configuration.GetValue(It.IsAny<string>()))
                .Options;
            return new ApplicationContext(options);
        }
    }
}
