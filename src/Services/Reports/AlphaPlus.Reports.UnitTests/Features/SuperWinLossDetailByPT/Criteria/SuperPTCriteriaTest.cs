using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

namespace AlphaPlus.Reports.UnitTests.Features.SuperWinLossDetailByPT;

[Trait("Category", "SuperWinLossDetailByPT")]
public class SuperPTCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        string superIds = "1,2,3";
        byte settingStatus = 1;

        // Act
        var criteria = new SuperPTCriteria(superIds, settingStatus);

        // Assert
        Assert.Equal(superIds, criteria.Supers);
        Assert.Equal(settingStatus, criteria.SettingStatus);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Expected_Key()
    {
        // Arrange
        var criteria = new SuperPTCriteria("1,2,3", 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("EAS_GetPTProductBySuper", result);
    }

    [Theory]
    [InlineData("1,2,3", true)]
    [InlineData("   ", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_Should_Return_Expected_Result(string superIds, bool expectedResult)
    {
        // Arrange
        var criteria = new SuperPTCriteria(superIds, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}