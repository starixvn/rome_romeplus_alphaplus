using AlphaPlus.RequestInfo.Criteria;

namespace AlphaPlus.RequestInfo.UnitTests;

public class GetIPInformationCriteriaTests
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetIPInformationCriteria { Ip = "192.168.1.1" };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_GetIPInfo_Full", result);
    }

    [Theory]
    [InlineData("192.168.1.1", true)]
    [InlineData("10.0.0.1", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", true)] // string.IsNullOrEmpty returns false for whitespace
    public void IsValid_ShouldReturnExpectedResult(string ip, bool expected)
    {
        // Arrange
        var criteria = new GetIPInformationCriteria { Ip = ip };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Ip_Property_ShouldBeSettable()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var criteria = new GetIPInformationCriteria { Ip = expectedIp };

        // Act & Assert
        Assert.Equal(expectedIp, criteria.Ip);
    }
}