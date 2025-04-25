using AlphaPlus.Reports.Features.Consolidation;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class GetConsolidationProductsCriteriaTest
{
    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetConsolidationProductsCriteria(1, 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_ConsolidatedGroupProductSel", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetConsolidationProductsCriteria(custId, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int expectedCustId = 123;
        byte expectedSettingStatus = 2;

        // Act
        var criteria = new GetConsolidationProductsCriteria(expectedCustId, expectedSettingStatus);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
        Assert.Equal(expectedSettingStatus, criteria.SettingStatus);
    }
}