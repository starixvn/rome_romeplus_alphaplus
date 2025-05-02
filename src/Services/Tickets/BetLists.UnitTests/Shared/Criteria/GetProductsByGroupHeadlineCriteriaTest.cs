using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetProductsByGroupHeadlineCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        const int expectedCustId = 123;
        const byte expectedSettingStatus = 1;

        // Act
        var criteria = new GetProductsByGroupHeadlineCriteria(expectedCustId, expectedSettingStatus);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
        Assert.Equal(expectedSettingStatus, criteria.SettingStatus);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new GetProductsByGroupHeadlineCriteria(1, 1);
        const string expectedKey = "Age_Rpt_ConsolidatedGroupProductSel";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetProductsByGroupHeadlineCriteria(custId, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
