using AlphaPlus.Reports.Features.Outstanding;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class OutstandingMemberCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new OutstandingMemberCriteria(1, "1,2,3");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_Outstanding_Member", result);
    }

    [Theory]
    [InlineData(1, "1,2,3", true)]
    [InlineData(1, "", false)]
    [InlineData(1, null, false)]
    [InlineData(1, "   ", false)]
    [InlineData(0, "1,2,3", false)]
    [InlineData(-1, "1,2,3", false)]
    [InlineData(0, "", false)]
    [InlineData(0, null, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, string productIds, bool expected)
    {
        // Arrange
        var criteria = new OutstandingMemberCriteria(custId, productIds);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        const int custId = 123;
        const string productIds = "1,2,3";
        var criteria = new OutstandingMemberCriteria(custId, productIds);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.IsAssignableFrom<CriteriaBase>(criteria);
    }
}