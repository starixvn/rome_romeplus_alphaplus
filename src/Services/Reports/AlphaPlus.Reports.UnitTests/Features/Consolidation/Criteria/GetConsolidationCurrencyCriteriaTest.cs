using AlphaPlus.Reports.Features.Consolidation;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class GetConsolidationCurrencyCriteriaTest
{
    [Fact]
    public void Constructor_Should_SetCustId_Correctly()
    {
        // Arrange
        var custId = 123;

        // Act
        var criteria = new GetConsolidationCurrencyCriteria(custId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CorrectKey()
    {
        // Arrange
        var criteria = new GetConsolidationCurrencyCriteria(123);
        var expectedKey = "Age_Rpt_ConsolidatedGroupSelCurrency";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetConsolidationCurrencyCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CustId_Should_Be_ReadOnly()
    {
        // Arrange
        var initialCustId = 123;
        var criteria = new GetConsolidationCurrencyCriteria(initialCustId);

        // Assert
        Assert.Equal(initialCustId, criteria.CustId);
        // Verify property only has a getter
        var property = typeof(GetConsolidationCurrencyCriteria).GetProperty("CustId");
        Assert.False(property.CanWrite);
        Assert.True(property.CanRead);
    }
}