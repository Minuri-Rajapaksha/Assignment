using Data.DbFactory.Application;
using Data.File;
using Data.Interfaces.File;
using Data.Interfaces.UnitOfWork.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Application;
using Service.Interfaces.Application;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Constants;
using Shared.Model.DB.Application;
using Shared.Model.ServerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass]
    public class AccountPeriodBalanceServiceTest : BaseTest
    {
        private IAccountPeriodBalanceService _accountPeriodBalanceService;
        private ApplicationDbFactory _applicationDbFactory;
        private Mock<IFileAccessor> _fileAccessor;
        private Mock<IImportProcess> _importProcess;

        #region Test fixture
        [TestInitialize]
        public override void Initialize()
        {
            _applicationDbFactory = CreateApplicationDbFactoryTest();
            _fileAccessor = new Mock<IFileAccessor>();
            _importProcess = new Mock<IImportProcess>();
            _accountPeriodBalanceService = new AccountPeriodBalanceService(_applicationDbFactory, _fileAccessor.Object, _importProcess.Object);
        }
        #endregion

        #region Test Methods
        [TestMethod]
        public async Task GetAccountBalanceForPeriodAsync()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("accountperiodbalanceservice-test-01");

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                var periodId = uow.Periods.GetAll().First(i => i.Discription == "January 2017").PeriodId;

                // Act
                var actualResult = await _accountPeriodBalanceService.GetAccountBalanceForPeriodAsync(periodId);

                // Assert
                Assert.IsTrue(actualResult.Count == 2, "Expected count is 2");
                Assert.IsTrue(actualResult.Any(i => i.AccountName == "R&D"), "Expected result should have R&D");
                Assert.IsTrue(!actualResult.Any(i => i.AccountName == ""), "Account name can not empty");
            }
        }

        [TestMethod]
        public async Task GetAccountBalanceForPeriodRangeAsync_AllAccount()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("accountperiodbalanceservice-test-02");

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                var StartPeriodId = uow.Periods.GetAll().First(i => i.Discription == "January 2017").PeriodId;
                var EndPeriodId = uow.Periods.GetAll().First(i => i.Discription == "February 2017").PeriodId;
                
                // Act
                var actualResult = await _accountPeriodBalanceService.GetAccountBalanceForPeriodRangeAsync(
                    new Shared.Model.WebClientModel.AccountBalancePeriodRangeModel
                    {
                        AccountId = 0,
                        StartPeriodId = StartPeriodId,
                        EndPeriodId = EndPeriodId
                    });

                // Assert
                Assert.IsTrue(actualResult.DataSet.Count == 3, "Expected count is 3");
                Assert.IsTrue(actualResult.Period.Count == 2, "Expected count is 2");
                Assert.IsTrue(actualResult.DataSet[0].Data.Count == 2, "Expected count is 2");
            }
        }

        [TestMethod]
        public async Task GetAccountBalanceForPeriodRangeAsync_OneAccount()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("accountperiodbalanceservice-test-03");

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                var accountId = uow.Accounts.GetAll().First(i => i.AccountName == "Canteen").AccountId;
                var StartPeriodId = uow.Periods.GetAll().First(i => i.Discription == "January 2017").PeriodId;
                var EndPeriodId = uow.Periods.GetAll().First(i => i.Discription == "February 2017").PeriodId;

                // Act
                var actualResult = await _accountPeriodBalanceService.GetAccountBalanceForPeriodRangeAsync(
                    new Shared.Model.WebClientModel.AccountBalancePeriodRangeModel
                    {
                        AccountId = accountId,
                        StartPeriodId = StartPeriodId,
                        EndPeriodId = EndPeriodId
                    });

                // Assert
                Assert.IsTrue(actualResult.DataSet.Count == 1, "Expected count is 1");
                Assert.IsTrue(actualResult.Period.Count == 2, "Expected count is 2");
                Assert.IsTrue(actualResult.DataSet[0].Data.Count == 2, "Expected count is 2");
            }
        }

        [TestMethod]
        public async Task UploadAndImportFile()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("accountperiodbalanceservice-test-04");
            _fileAccessor.Setup(c => c.WriteFileAsync(It.IsAny<Stream>(), It.IsAny<FileUploadModel>())).Returns(Task.FromResult(true));
            _importProcess.Setup(c => c.ProcessFileAsync(It.IsAny<FileUploadModel>())).Returns(Task.FromResult(0));


            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                // Act
                var actualResult = await _accountPeriodBalanceService.UploadAndImportFile(1, null, "testFile.txt", 1);

                // Assert
                Assert.IsTrue(actualResult, "UploadAndImportFile error");
            }
        }
        #endregion

        #region data seeding
        private async Task SeedAccounts(IUnitOfWorkApplication uow)
        {
            var rAndD = uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "R&D"
            });
            var canteen = uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "Canteen"
            });
            var ceoCar = uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "CEOs Car"
            });

            var jan = uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "January 2017",
                PeriodDate = new DateTime(2017, 01, 01)
            });
            var feb = uow.Periods.Insert(new Period
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                Discription = "February 2017",
                PeriodDate = new DateTime(2017, 02, 01)
            });
            await uow.SaveAsync();

            uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
            {
                AccountId = rAndD.AccountId,
                Balance = 10.56M,
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                PeriodId = jan.PeriodId,
            });
            uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
            {
                AccountId = rAndD.AccountId,
                Balance = 188.56M,
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                PeriodId = feb.PeriodId,
            });
            uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
            {
                AccountId = canteen.AccountId,
                Balance = 77.56M,
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                PeriodId = jan.PeriodId,
            });
            uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
            {
                AccountId = canteen.AccountId,
                Balance = 99M,
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                PeriodId = feb.PeriodId,
            });
            await uow.SaveAsync();
        }
        #endregion
    }
}
