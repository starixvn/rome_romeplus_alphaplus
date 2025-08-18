using AlphaPlus.Utilities.IP.Criteria;

namespace AlphaPlus.Utilities.IP.UnitTests
{
    public class GetIPInformationCriteriaTests
    {
        [Fact]
        public void Constructor_WithValidIP_SetsIpProperty()
        {
            // Arrange
            var expectedIp = "192.168.1.1";

            // Act
            var criteria = new GetIPInformationCriteria(expectedIp);

            // Assert
            Assert.Equal(expectedIp, criteria.Ip);
        }

        [Fact]
        public void Constructor_WithEmptyIP_SetsIpProperty()
        {
            // Arrange
            var expectedIp = string.Empty;

            // Act
            var criteria = new GetIPInformationCriteria(expectedIp);

            // Assert
            Assert.Equal(expectedIp, criteria.Ip);
        }

        [Fact]
        public void Constructor_WithNullIP_SetsIpProperty()
        {
            // Arrange
            string expectedIp = null;

            // Act
            var criteria = new GetIPInformationCriteria(expectedIp!);

            // Assert
            Assert.Equal(expectedIp, criteria.Ip);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        [InlineData("172.16.0.1")]
        [InlineData("8.8.8.8")]
        [InlineData("1.1.1.1")]
        [InlineData("::1")]
        [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        public void GetSettingKey_ReturnsExpectedKey(string ip)
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(ip);
            var expectedKey = "Acc_GetIPInfo_Full";

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Theory]
        [InlineData("192.168.1.1", true)]
        [InlineData("10.0.0.1", true)]
        [InlineData("172.16.0.1", true)]
        [InlineData("8.8.8.8", true)]
        [InlineData("1.1.1.1", true)]
        [InlineData("::1", true)]
        [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true)]
        [InlineData("", false)]
        [InlineData("   ", true)]
        [InlineData(null, false)]
        public void IsValid_WithVariousIPs_ReturnsExpectedResult(string ip, bool expected)
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(ip);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsValid_WithWhitespaceOnlyIP_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("   ");

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WithTabOnlyIP_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("\t");

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WithNewlineOnlyIP_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("\n");

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("10.0.0.1")]
        [InlineData("172.16.0.1")]
        [InlineData("8.8.8.8")]
        [InlineData("1.1.1.1")]
        [InlineData("::1")]
        [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        public void IsValid_WithValidIPs_ReturnsTrue(string ip)
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(ip);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_WithInvalidIPs_ReturnsFalse(string ip)
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(ip);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Ip_Property_IsReadOnly()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("192.168.1.1");

            // Act & Assert
            var propertyInfo = typeof(GetIPInformationCriteria).GetProperty("Ip");
            Assert.NotNull(propertyInfo);
            Assert.False(propertyInfo.CanWrite);
            Assert.True(propertyInfo.CanRead);
        }

        [Fact]
        public void GetSettingKey_AlwaysReturnsSameValue()
        {
            // Arrange
            var criteria1 = new GetIPInformationCriteria("192.168.1.1");
            var criteria2 = new GetIPInformationCriteria("10.0.0.1");
            var criteria3 = new GetIPInformationCriteria("");

            // Act
            var result1 = criteria1.GetSettingKey();
            var result2 = criteria2.GetSettingKey();
            var result3 = criteria3.GetSettingKey();

            // Assert
            Assert.Equal("Acc_GetIPInfo_Full", result1);
            Assert.Equal("Acc_GetIPInfo_Full", result2);
            Assert.Equal("Acc_GetIPInfo_Full", result3);
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }
    }
}