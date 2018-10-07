
using Data.DbFactory.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Tests.Interfaces;
using Shared.Constants;

namespace Service.Tests
{
    public class BaseTest
    {
        protected Mock<IConfiguration> _mockConfiguration;

        public BaseTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            // each unit test wise database should re-create 
            _mockConfiguration.Setup(c => c.GetValue(ConnectionStrings.ApplicationConnection)).Returns("");
        }

        #region Test fixture
        [TestInitialize()]
        public virtual void Initialize()
        {
            // Initialize
        }

        [TestCleanup()]
        public virtual void Cleanup()
        {
            // Cleanup 
        }
        #endregion

        #region common methods
        public ApplicationDbFactory CreateApplicationDbFactoryTest()
        {
            return new ApplicationDbFactoryTest(_mockConfiguration.Object);
        }


        #endregion
    }
}
