using AlphaPlus.Reports.Features.Customer;

namespace AlphaPlus.Reports.UnitTests.Features.Customer;

[Trait("Category", "Customer")]
public class GetCustomerLevelCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        const int uplineId = 123;
        const string username = "testUser";
        var criteria = new GetCustomerLevelCriteria(uplineId, username);

        // Assert
        Assert.Equal(uplineId, criteria.UplineId);
        Assert.Equal(username, criteria.Username);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetCustomerLevelCriteria(1, "test");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_GetCustomerInfo", result);
    }

    [Theory]
    [InlineData(1, "user", true)]
    [InlineData(0, "user", false)]
    [InlineData(-1, "user", false)]
    [InlineData(1, "", false)]
    [InlineData(1, " ", false)]
    [InlineData(1, null, false)]
    public void IsValid_ShouldReturnExpectedResult(int uplineId, string username, bool expectedResult)
    {
        // Arrange
        var criteria = new GetCustomerLevelCriteria(uplineId, username);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}