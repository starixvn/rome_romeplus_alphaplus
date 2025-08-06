using AlphaPlus.SmartPunter.Criteria;
using AlphaPlus.SmartPunter.DataAccesses;
using AlphaPlus.SmartPunter.Models;
using AlphaPlus.SmartPunter.Services;
using AutoFixture;
using Moq;

namespace AlphaPlus.SmartPunter.UnitTests.Services
{
    public class SmartPunterServiceTests
    {
        private readonly Mock<ISmartPunterDataAccess> _mockDataAccess;
        private readonly SmartPunterService _service;
        private readonly Fixture _fixture;

        public SmartPunterServiceTests()
        {
            _mockDataAccess = new Mock<ISmartPunterDataAccess>();
            _service = new SmartPunterService(_mockDataAccess.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void CheckInternalAdmin_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            _service.CheckInternalAdmin(customerId);

            // Assert
            _mockDataAccess.Verify(x => x.CheckInternalAdmin(It.Is<InternalAdminChecker>(c => c.CustId == customerId)), Times.Once);
        }

        [Fact]
        public void CheckInternalAdmin_ShouldReturnDataAccessResult()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            bool expectedResult = _fixture.Create<bool>();
            _mockDataAccess.Setup(x => x.CheckInternalAdmin(It.IsAny<InternalAdminChecker>())).Returns(expectedResult);

            // Act
            bool result = _service.CheckInternalAdmin(customerId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CheckSmartPunter_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            int customerId = _fixture.Create<int>();

            // Act
            _service.CheckSmartPunter(customerId);

            // Assert
            _mockDataAccess.Verify(x => x.CheckSmartPunter(It.Is<SmartPunterSMAChecker>(c => c.CustId == customerId)), Times.Once);
        }

        [Fact]
        public void CheckSmartPunter_ShouldReturnDataAccessResult()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            var expectedCustomer = new SMACustomer(true, false);
            _mockDataAccess.Setup(x => x.CheckSmartPunter(It.IsAny<SmartPunterSMAChecker>())).Returns(expectedCustomer);

            // Act
            var result = _service.CheckSmartPunter(customerId);

            // Assert
            Assert.Equal(expectedCustomer, result);
        }

        [Fact]
        public void WriteLog_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            string userNames = _fixture.Create<string>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            _service.WriteLog(userNames, isRestricted, userId, ip);

            // Assert
            _mockDataAccess.Verify(x => x.WriteLog(It.Is<SmartPunterWriteLog>(c =>
                c.ListUserName == userNames &&
                c.ActionId == 31008 &&
                c.IsRestricted == isRestricted &&
                c.UserId == userId &&
                c.IP == ip)), Times.Once);
        }

        [Fact]
        public void CheckSmartPunterAndWriteLog_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            string userNames = _fixture.Create<string>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            _service.CheckSmartPunterAndWriteLog(userNames, isRestricted, userId, ip);

            // Assert
            _mockDataAccess.Verify(x => x.CheckSmartPunterAndWriteLog(It.Is<SmartPunterSMACheckerWriteLog>(c =>
                c.ListUserName == userNames &&
                c.ActionId == 31008 &&
                c.IsRestricted == isRestricted &&
                c.UserId == userId &&
                c.IP == ip)), Times.Once);
        }

        [Fact]
        public void WriteLogNoData_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            int? customerId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            _service.WriteLogNoData(customerId, isRestricted, userId, ip);

            // Assert
            _mockDataAccess.Verify(x => x.WriteLogNoData(It.Is<SmartPunterWriteLogNoData>(c =>
                c.CustId == customerId &&
                c.ActionId == 31008 &&
                c.IsRestricted == isRestricted &&
                c.UserId == userId &&
                c.IP == ip)), Times.Once);
        }

        [Fact]
        public void WriteLogNoData_WithNullCustomerId_ShouldCallDataAccessWithNullCustomerId()
        {
            // Arrange
            int? customerId = null;
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            _service.WriteLogNoData(customerId, isRestricted, userId, ip);

            // Assert
            _mockDataAccess.Verify(x => x.WriteLogNoData(It.Is<SmartPunterWriteLogNoData>(c =>
                c.CustId == null &&
                c.ActionId == 31008 &&
                c.IsRestricted == isRestricted &&
                c.UserId == userId &&
                c.IP == ip)), Times.Once);
        }

        [Fact]
        public void HasSmartPunterPermission_ShouldCallDataAccessWithCorrectCriteria()
        {
            // Arrange
            int loggedInUserId = _fixture.Create<int>();

            // Act
            _service.HasSmartPunterPermission(loggedInUserId);

            // Assert
            _mockDataAccess.Verify(x => x.CheckPermission(It.Is<NAPUserPermission>(c => c.CustId == loggedInUserId), "[SmartPunter]"), Times.Once);
        }

        [Fact]
        public void HasSmartPunterPermission_ShouldReturnDataAccessResult()
        {
            // Arrange
            int loggedInUserId = _fixture.Create<int>();
            bool expectedResult = _fixture.Create<bool>();
            _mockDataAccess.Setup(x => x.CheckPermission(It.IsAny<NAPUserPermission>(), It.IsAny<string>())).Returns(expectedResult);

            // Act
            bool result = _service.HasSmartPunterPermission(loggedInUserId);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}