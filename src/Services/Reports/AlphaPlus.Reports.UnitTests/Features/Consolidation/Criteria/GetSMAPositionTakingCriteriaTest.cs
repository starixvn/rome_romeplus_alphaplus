using AlphaPlus.Reports.Features.Consolidation;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class GetSMAPositionTakingCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        const string custIds = "1,2,3";
        const byte settingStatus = 1;

        // Act
        var criteria = new GetSMAPositionTakingCriteria(custIds, settingStatus);

        // Assert
        Assert.Equal(custIds, criteria.CustIds);
        Assert.Equal(settingStatus, criteria.SettingStatus);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetSMAPositionTakingCriteria("1", 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("EAS_GetPTProductBySMA", result);
    }

    [Theory]
    [InlineData("1,2,3", true)]
    [InlineData("123", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    public void IsValid_Should_Return_Expected_Result(string custIds, bool expected)
    {
        // Arrange
        var criteria = new GetSMAPositionTakingCriteria(custIds, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}