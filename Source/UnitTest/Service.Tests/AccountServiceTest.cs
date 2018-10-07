using Data.DbFactory.Application;
using Data.Interfaces.UnitOfWork.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Application;
using Service.Interfaces.Application;
using Shared.Constants;
using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass]
    public class AccountServiceTest : BaseTest
    {
        private IAccountService _accountService;
        private ApplicationDbFactory _applicationDbFactory;

        #region Test fixture
        [TestInitialize]
        public override void Initialize()
        {
            _applicationDbFactory = CreateApplicationDbFactoryTest();
            _accountService = new AccountService(_applicationDbFactory);
        }
        #endregion

        #region Test Methods
        [TestMethod]
        public async Task GetAccountDropdownListAsync()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(ConnectionStrings.ApplicationConnection)).Returns("period-test-01");

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                // Act
                var actualResult = await _accountService.GetAccountDropdownListAsync();

                // Assert
                Assert.IsTrue(actualResult.Count == 3, "Expected count is 3");
                Assert.IsTrue(actualResult.Any(i => i.AccountName == "R&D"), "Expected result should have R&D");
            }
        }
        #endregion

        #region data seeding
        private async Task SeedAccounts(IUnitOfWorkApplication uow)
        {
            uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "R&D"
            });
            uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "Canteen"
            });
            uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "CEOs Car"
            });
        }
        #endregion
    }
}
