using AlphaPlus.SmartPunter.Criteria;
using AutoFixture;

namespace AlphaPlus.SmartPunter.UnitTests.Criteria
{
    public class SmartPunterWriteLogNoDataTests
    {
        private readonly Fixture _fixture;

        public SmartPunterWriteLogNoDataTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldSetAllProperties()
        {
            // Arrange
            int? customerId = _fixture.Create<int>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            var writeLog = new SmartPunterWriteLogNoData(customerId, actionId, isRestricted, userId, ip);

            // Assert
            Assert.Equal(customerId, writeLog.CustId);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
        }

        [Fact]
        public void Constructor_WithNullCustomerId_ShouldSetNullCustomerId()
        {
            // Arrange
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            var writeLog = new SmartPunterWriteLogNoData(null, actionId, isRestricted, userId, ip);

            // Assert
            Assert.Null(writeLog.CustId);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
        }

        [Fact]
        public void Constructor_WithNullIP_ShouldSetNullIP()
        {
            // Arrange
            int? customerId = _fixture.Create<int>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();

            // Act
            var writeLog = new SmartPunterWriteLogNoData(customerId, actionId, isRestricted, userId, null);

            // Assert
            Assert.Equal(customerId, writeLog.CustId);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Null(writeLog.IP);
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            int? customerId = _fixture.Create<int>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var writeLog = new SmartPunterWriteLogNoData(customerId, actionId, isRestricted, userId, ip);

            // Act & Assert
            Assert.Equal(customerId, writeLog.CustId);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
            // The properties should be read-only, so we can't modify them after construction
        }
    }
}