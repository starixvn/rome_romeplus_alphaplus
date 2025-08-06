using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Utilities.IP;
using AutoFixture;
using Fanex.BetList.Core.Entities;
using Moq;

namespace AlphaPlus.SmartPunter.UnitTests.Services
{
    public class SmartPunterBetListMonitorTests
    {
        private readonly Mock<IUserSession> _mockUserSession;
        private readonly Mock<ISmartPunterService> _mockSmartPunterService;
        private readonly Mock<IPHelper> _mockIpHelper;
        private readonly SmartPunterBetListMonitor _monitor;
        private readonly Fixture _fixture;

        public SmartPunterBetListMonitorTests()
        {
            _mockUserSession = new Mock<IUserSession>();
            _mockSmartPunterService = new Mock<ISmartPunterService>();
            _mockIpHelper = new Mock<IPHelper>();
            _monitor = new SmartPunterBetListMonitor(_mockUserSession.Object, _mockSmartPunterService.Object, _mockIpHelper.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void WriteLog_WhenNotInternalUser_ShouldReturnEarly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(false);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.CheckInternalAdmin(It.IsAny<int>()), Times.Never);
            _mockSmartPunterService.Verify(x => x.CheckSmartPunter(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void WriteLog_WhenNotInternalAdmin_ShouldReturnEarly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(false);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.CheckSmartPunter(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void WriteLog_WhenCustomerIsNotSmartPunterAndNoDownlineSmartPunter_ShouldReturnEarly()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(false, false));

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockSmartPunterService.Verify(x => x.WriteLogNoData(It.IsAny<int?>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void WriteLog_WhenCustomerIsSmartPunterWithTickets_ShouldWriteLog()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(true, false));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(false);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLog("user1,user2", true, userId, ip), Times.Once);
        }

        [Fact]
        public void WriteLog_WhenCustomerIsSmartPunterWithoutTickets_ShouldWriteLogNoData()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(true, false));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(false);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, null);

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLogNoData(customerId, true, userId, ip), Times.Once);
        }

        [Fact]
        public void WriteLog_WhenCustomerHasDownlineSmartPunterWithTickets_ShouldCheckSmartPunterAndWriteLog()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(false, true));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(false);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.CheckSmartPunterAndWriteLog("user1,user2", true, userId, ip), Times.Once);
        }

        [Fact]
        public void WriteLog_WhenCustomerHasDownlineSmartPunterWithoutTickets_ShouldWriteLogNoDataWithNullCustomerId()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(false, true));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(false);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, new List<ITicket>());

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLogNoData(null, true, userId, ip), Times.Once);
        }

        [Fact]
        public void WriteLog_WhenUserHasSmartPunterPermission_ShouldSetIsRestrictedToFalse()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(true, false));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(true);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLog("user1,user2", false, userId, ip), Times.Once);
        }

        [Fact]
        public void WriteLog_WhenCustomerIsBothSmartPunterAndHasDownlineSmartPunter_ShouldPrioritizeSmartPunter()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var tickets = CreateMockTickets();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns(new SMACustomer(true, true));
            _mockSmartPunterService.Setup(x => x.HasSmartPunterPermission(userId)).Returns(false);
            _mockIpHelper.Setup(x => x.GetClientIP()).Returns(ip);

            // Act
            _monitor.WriteLog(customerId, tickets);

            // Assert
            _mockSmartPunterService.Verify(x => x.WriteLog("user1,user2", true, userId, ip), Times.Once);
            _mockSmartPunterService.Verify(x => x.CheckSmartPunterAndWriteLog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void WriteLog_WhenCustomerIsNull_ShouldHandleGracefully()
        {
            // Arrange
            int customerId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            _mockUserSession.Setup(x => x.IsInternal).Returns(true);
            _mockUserSession.Setup(x => x.AdminId).Returns(userId);
            _mockSmartPunterService.Setup(x => x.CheckInternalAdmin(userId)).Returns(true);
            _mockSmartPunterService.Setup(x => x.CheckSmartPunter(customerId)).Returns((SMACustomer)null);

            // Act
            _monitor.WriteLog(customerId, new List<ITicket>());

            // Assert
            // Should not throw any exception and should return early since customer is null
            _mockSmartPunterService.Verify(x => x.WriteLog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockSmartPunterService.Verify(x => x.WriteLogNoData(It.IsAny<int?>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        private List<ITicket> CreateMockTickets()
        {
            var mockTicket1 = new Mock<ITicket>();
            mockTicket1.Setup(x => x.UserName).Returns("user1");

            var mockTicket2 = new Mock<ITicket>();
            mockTicket2.Setup(x => x.UserName).Returns("user2");

            var mockTicket3 = new Mock<ITicket>();
            mockTicket3.Setup(x => x.UserName).Returns("user1"); // Duplicate username

            return new List<ITicket> { mockTicket1.Object, mockTicket2.Object, mockTicket3.Object };
        }
    }
}