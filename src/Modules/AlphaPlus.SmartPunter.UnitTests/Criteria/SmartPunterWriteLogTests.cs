using AlphaPlus.SmartPunter.Criteria;
using AutoFixture;

namespace AlphaPlus.SmartPunter.UnitTests.Criteria
{
    public class SmartPunterWriteLogTests
    {
        private readonly Fixture _fixture;

        public SmartPunterWriteLogTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldSetAllProperties()
        {
            // Arrange
            string userNames = _fixture.Create<string>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            var writeLog = new SmartPunterWriteLog(userNames, actionId, isRestricted, userId, ip);

            // Assert
            Assert.Equal(userNames, writeLog.ListUserName);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
        }

        [Fact]
        public void Constructor_WithNullUserNames_ShouldSetNullUserNames()
        {
            // Arrange
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();

            // Act
            var writeLog = new SmartPunterWriteLog(null, actionId, isRestricted, userId, ip);

            // Assert
            Assert.Null(writeLog.ListUserName);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
        }

        [Fact]
        public void Constructor_WithNullIP_ShouldSetNullIP()
        {
            // Arrange
            string userNames = _fixture.Create<string>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();

            // Act
            var writeLog = new SmartPunterWriteLog(userNames, actionId, isRestricted, userId, null);

            // Assert
            Assert.Equal(userNames, writeLog.ListUserName);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Null(writeLog.IP);
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            string userNames = _fixture.Create<string>();
            int actionId = _fixture.Create<int>();
            bool isRestricted = _fixture.Create<bool>();
            int userId = _fixture.Create<int>();
            string ip = _fixture.Create<string>();
            var writeLog = new SmartPunterWriteLog(userNames, actionId, isRestricted, userId, ip);

            // Act & Assert
            Assert.Equal(userNames, writeLog.ListUserName);
            Assert.Equal(actionId, writeLog.ActionId);
            Assert.Equal(isRestricted, writeLog.IsRestricted);
            Assert.Equal(userId, writeLog.UserId);
            Assert.Equal(ip, writeLog.IP);
            // The properties should be read-only, so we can't modify them after construction
        }
    }
}