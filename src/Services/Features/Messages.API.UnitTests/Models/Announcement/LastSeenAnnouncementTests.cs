using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models.Announcement
{
    public class LastSeenAnnouncementTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithMinValues()
        {
            // Act
            var lastSeen = new LastSeenAnnouncement();

            // Assert
            Assert.Equal(0, lastSeen.CustId);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenNormal);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenPersonal);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenSpecial);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenSystem);
        }

        [Fact]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var custId = 123;
            var normalDate = new DateTime(2024, 1, 1);
            var personalDate = new DateTime(2024, 1, 2);
            var specialDate = new DateTime(2024, 1, 3);
            var systemDate = new DateTime(2024, 1, 4);

            // Act
            var lastSeen = new LastSeenAnnouncement(custId, normalDate, personalDate, specialDate, systemDate);

            // Assert
            Assert.Equal(custId, lastSeen.CustId);
            Assert.Equal(normalDate, lastSeen.LastSeenNormal);
            Assert.Equal(personalDate, lastSeen.LastSeenPersonal);
            Assert.Equal(specialDate, lastSeen.LastSeenSpecial);
            Assert.Equal(systemDate, lastSeen.LastSeenSystem);
        }

        [Fact]
        public void CustId_CanBeSetAndRetrieved()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var custId = 456;

            // Act
            lastSeen.CustId = custId;

            // Assert
            Assert.Equal(custId, lastSeen.CustId);
        }

        [Fact]
        public void LastSeenNormal_CanBeSetAndRetrieved()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date = new DateTime(2024, 5, 15);

            // Act
            lastSeen.LastSeenNormal = date;

            // Assert
            Assert.Equal(date, lastSeen.LastSeenNormal);
        }

        [Fact]
        public void LastSeenPersonal_CanBeSetAndRetrieved()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date = new DateTime(2024, 5, 16);

            // Act
            lastSeen.LastSeenPersonal = date;

            // Assert
            Assert.Equal(date, lastSeen.LastSeenPersonal);
        }

        [Fact]
        public void LastSeenSpecial_CanBeSetAndRetrieved()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date = new DateTime(2024, 5, 17);

            // Act
            lastSeen.LastSeenSpecial = date;

            // Assert
            Assert.Equal(date, lastSeen.LastSeenSpecial);
        }

        [Fact]
        public void LastSeenSystem_CanBeSetAndRetrieved()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date = new DateTime(2024, 5, 18);

            // Act
            lastSeen.LastSeenSystem = date;

            // Assert
            Assert.Equal(date, lastSeen.LastSeenSystem);
        }

        [Theory]
        [InlineData(MessageType.Normal)]
        [InlineData(MessageType.Personal)]
        [InlineData(MessageType.Special)]
        [InlineData(MessageType.System)]
        public void GetLastSeenByType_WithValidMessageType_ReturnsCorrectDate(int messageType)
        {
            // Arrange
            var normalDate = new DateTime(2024, 1, 1);
            var personalDate = new DateTime(2024, 1, 2);
            var specialDate = new DateTime(2024, 1, 3);
            var systemDate = new DateTime(2024, 1, 4);

            var lastSeen = new LastSeenAnnouncement(123, normalDate, personalDate, specialDate, systemDate);

            // Act
            var result = lastSeen.GetLastSeenByType(messageType);

            // Assert
            var expected = messageType switch
            {
                MessageType.Normal => normalDate,
                MessageType.Personal => personalDate,
                MessageType.Special => specialDate,
                MessageType.System => systemDate,
                _ => throw new ArgumentException("Invalid message type")
            };

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(MessageType.Normal)]
        [InlineData(MessageType.Personal)]
        [InlineData(MessageType.Special)]
        [InlineData(MessageType.System)]
        public void GetLastSeenByType_AfterSettingProperty_ReturnsUpdatedDate(int messageType)
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var newDate = new DateTime(2024, 12, 25);

            // Act
            switch (messageType)
            {
                case MessageType.Normal:
                    lastSeen.LastSeenNormal = newDate;
                    break;
                case MessageType.Personal:
                    lastSeen.LastSeenPersonal = newDate;
                    break;
                case MessageType.Special:
                    lastSeen.LastSeenSpecial = newDate;
                    break;
                case MessageType.System:
                    lastSeen.LastSeenSystem = newDate;
                    break;
            }

            var result = lastSeen.GetLastSeenByType(messageType);

            // Assert
            Assert.Equal(newDate, result);
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date1 = new DateTime(2024, 1, 1);
            var date2 = new DateTime(2024, 2, 2);
            var date3 = new DateTime(2024, 3, 3);
            var date4 = new DateTime(2024, 4, 4);

            // Act
            lastSeen.LastSeenNormal = date1;
            lastSeen.LastSeenPersonal = date2;
            lastSeen.LastSeenSpecial = date3;
            lastSeen.LastSeenSystem = date4;

            // Assert
            Assert.Equal(date1, lastSeen.LastSeenNormal);
            Assert.Equal(date2, lastSeen.LastSeenPersonal);
            Assert.Equal(date3, lastSeen.LastSeenSpecial);
            Assert.Equal(date4, lastSeen.LastSeenSystem);

            // Verify they don't affect each other
            Assert.NotEqual(lastSeen.LastSeenNormal, lastSeen.LastSeenPersonal);
            Assert.NotEqual(lastSeen.LastSeenPersonal, lastSeen.LastSeenSpecial);
            Assert.NotEqual(lastSeen.LastSeenSpecial, lastSeen.LastSeenSystem);
        }

        [Fact]
        public void GetLastSeenByType_WithInvalidMessageType_ThrowsKeyNotFoundException()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var invalidMessageType = 999;

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => lastSeen.GetLastSeenByType(invalidMessageType));
        }

        [Fact]
        public void PropertySetter_WithSameDateMultipleTimes_UpdatesCorrectly()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();
            var date1 = new DateTime(2024, 1, 1);
            var date2 = new DateTime(2024, 2, 2);

            // Act
            lastSeen.LastSeenNormal = date1;
            Assert.Equal(date1, lastSeen.LastSeenNormal);

            lastSeen.LastSeenNormal = date2;
            Assert.Equal(date2, lastSeen.LastSeenNormal);
        }

        [Fact]
        public void AllProperties_CanHandleMinAndMaxValues()
        {
            // Arrange
            var lastSeen = new LastSeenAnnouncement();

            // Act & Assert - Min Values
            lastSeen.LastSeenNormal = DateTime.MinValue;
            lastSeen.LastSeenPersonal = DateTime.MinValue;
            lastSeen.LastSeenSpecial = DateTime.MinValue;
            lastSeen.LastSeenSystem = DateTime.MinValue;

            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenNormal);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenPersonal);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenSpecial);
            Assert.Equal(DateTime.MinValue, lastSeen.LastSeenSystem);

            // Act & Assert - Max Values
            lastSeen.LastSeenNormal = DateTime.MaxValue;
            lastSeen.LastSeenPersonal = DateTime.MaxValue;
            lastSeen.LastSeenSpecial = DateTime.MaxValue;
            lastSeen.LastSeenSystem = DateTime.MaxValue;

            Assert.Equal(DateTime.MaxValue, lastSeen.LastSeenNormal);
            Assert.Equal(DateTime.MaxValue, lastSeen.LastSeenPersonal);
            Assert.Equal(DateTime.MaxValue, lastSeen.LastSeenSpecial);
            Assert.Equal(DateTime.MaxValue, lastSeen.LastSeenSystem);
        }

        [Fact]
        public void Constructor_WithNegativeCustId_AcceptsValue()
        {
            // Arrange & Act
            var lastSeen = new LastSeenAnnouncement(-1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Assert
            Assert.Equal(-1, lastSeen.CustId);
        }
    }
}
