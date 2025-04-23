using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class GetLogActionsCriteriaTests
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetLogActionsCriteria();

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_UserLog_LogActionSel", settingKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new GetLogActionsCriteria();

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }
}
