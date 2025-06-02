using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetIPV4InfoCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeIPListProperty()
    {
        // Arrange & Act
        const string expectedIpList = "192.168.1.1,10.0.0.1,172.16.0.1";
        var criteria = new GetIPV4InfoCriteria(expectedIpList);

        // Assert
        Assert.Equal(expectedIpList, criteria.IPList);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetIPV4InfoCriteria("192.168.1.1");
        var expectedKey = "Age_IPRangesByCountrySelByIPList";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenIPListIsNotEmpty()
    {
        // Arrange
        var criteria = new GetIPV4InfoCriteria("192.168.1.1");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldReturnFalse_WhenIPListIsNullOrWhiteSpace(string ipList)
    {
        // Arrange
        var criteria = new GetIPV4InfoCriteria(ipList);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}