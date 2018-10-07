
using Data.DbFactory.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Application;
using Microsoft.Extensions.Configuration;
using Service.Interfaces.Application;
using Shared.Constants;
using Data.Interfaces.UnitOfWork.Application;
using Shared.Model.DB.Application;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Service.Tests
{
    [TestClass]
    public class PeriodServiceTests : BaseTest
    {
        private IPeriodService _periodService;
        private ApplicationDbFactory _applicationDbFactory;

        #region Test fixture
        [TestInitialize()]
        public override void Initialize()
        {
            _applicationDbFactory = CreateApplicationDbFactoryTest();
            _periodService = new PeriodService(_applicationDbFactory);
        }
        #endregion

        #region Test methods
        [TestMethod]
        public async Task GetPeriodDropdownListAsync()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(ConnectionStrings.ApplicationConnection)).Returns("period-test-01");
            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedPeriods(uow);

                // Act
                var actualResult = await _periodService.GetPeriodDropdownListAsync();

                // Assert
                Assert.IsTrue(actualResult.Count == 4, "Expected count should be 4");
                Assert.IsTrue(actualResult.Any(i => i.Discription == "2018 - April"), "Expected result should have april record");
            }
        }
        #endregion

        #region data seeding
        private async Task SeedPeriods(IUnitOfWorkApplication uow)
        {
            uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "2018 January",
                PeriodDate = new DateTime(2018, 01, 01)
            });
            uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "2018 February",
                PeriodDate = new DateTime(2018, 02, 01)
            });
            uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "2018 March",
                PeriodDate = new DateTime(2018, 03, 01)
            });
            uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "2018 April",
                PeriodDate = new DateTime(2018, 04, 01)
            });
            await uow.SaveAsync();
        }
        #endregion
    }
}
