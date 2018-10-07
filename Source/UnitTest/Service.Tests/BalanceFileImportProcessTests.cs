
using Data.DbFactory.Application;
using Data.Interfaces.File;
using Data.Interfaces.UnitOfWork.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Application.BalanceFileUpload;
using Service.Interfaces.Application.BalanceFileUpload;
using Service.Tests.Interfaces;
using Shared.Model.DB.Application;
using Shared.Model.ServerModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass]
    public class BalanceFileImportProcessTests : BaseTest
    {
        private IBalanceFileImportProcess _balanceFileImportProcess;
        private Mock<IFileAccessor> _fileAccessor;
        private Mock<IConfiguration> _configuration;
        private ApplicationDbFactory _applicationDbFactory;

        #region Test fixture
        [TestInitialize()]
        public override void Initialize()
        {
            _applicationDbFactory = CreateApplicationDbFactoryTest();
            _configuration = new Mock<IConfiguration>();
            _fileAccessor = new Mock<IFileAccessor>();
            _balanceFileImportProcess = new BalanceFileImportProcess(_fileAccessor.Object, _configuration.Object, _applicationDbFactory);
        }
        #endregion

        #region Test methods
        [TestMethod]
        public async Task GetPeriodDropdownListAsync_ImportToUnKnownAccount()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("balancefileupload-test-01");
            _fileAccessor.Setup(c => c.ReadFileAsync(It.IsAny<FileUploadModel>())).Returns(Task.FromResult(new string[] {
                "AccountName 01 \t 45.56",
                "AccountName 02 \t 55.56",
                "AccountName 03 \t 11.56",
                "AccountName 04 \t 36.56",
                "AccountName 05 \t 99",
            }));

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                var recordCountBeforeImport = uow.AccountPeriodBalances.GetAll().Count();

                // Act
                var msg = new FileUploadModel
                {
                    Extension = "txt",
                    FileName = "testFile",
                    FileType = Shared.Enum.FileType.UploadDocument,
                    PeriodId = 1
                };
                await _balanceFileImportProcess.ProcessAsync(msg);

                // Assert
                var recordCountAfteImport = uow.AccountPeriodBalances.GetAll().Count();
                Assert.IsTrue(recordCountBeforeImport == 0, "Record count before import should be zero");
                Assert.IsTrue(recordCountAfteImport == 0, "Record count after import should be zero");
            }
        }

        [TestMethod]
        public async Task GetPeriodDropdownListAsync_ImportToKnownAccount()
        {
            // Arrange
            _mockConfiguration.Setup(c => c.GetValue(It.IsAny<string>())).Returns("balancefileupload-test-02");
            _fileAccessor.Setup(c => c.ReadFileAsync(It.IsAny<FileUploadModel>())).Returns(Task.FromResult(new string[] {
                "AccountName 01 \t 45.56",
                "AccountName 02 \t 55.56",
                "AccountName 03 \t 11.56"
            }));

            using (var uow = _applicationDbFactory.BeginUnitOfWorkAsync().Result)
            {
                await SeedAccounts(uow);
                var recordCountBeforeImport = uow.AccountPeriodBalances.GetAll().Count();

                // Act
                var msg = new FileUploadModel
                {
                    Extension = "txt",
                    FileName = "testFile",
                    FileType = Shared.Enum.FileType.UploadDocument,
                    PeriodId = 1
                };
                await _balanceFileImportProcess.ProcessAsync(msg);

                // Assert
                var recordCountAfteImport = uow.AccountPeriodBalances.GetAll().Count();
                Assert.IsTrue(recordCountBeforeImport == 0, "Record count before import should be zero");
                Assert.IsTrue(recordCountAfteImport == 3, "Record count after import should be zero");
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
                AccountName = "AccountName 01"
            });
            uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "AccountName 02"
            });
            uow.Accounts.Insert(new Account
            {
                CreatedBy = 1,
                CreatedDate = DateTimeOffset.Now,
                AccountName = "AccountName 03"
            });
            await uow.SaveAsync();
        }
        #endregion
    }
}
