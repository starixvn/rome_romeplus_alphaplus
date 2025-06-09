using AgencyCash.API.Shared.Criteria;

namespace AgencyCash.API.Tests.Shared.Criteria;

[Trait("Category", "Shared")]
public class GetSiteDBConfigCriteriaTests
{
    [Fact]
    public void Constructor_ShouldSetCustId()
    {
        // Arrange
        const int expectedCustId = 123;

        // Act
        var criteria = new GetSiteDBConfigCriteria(expectedCustId);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_ServerDBConfigSelByCust", result);
    }
}