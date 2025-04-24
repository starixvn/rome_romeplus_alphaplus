using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class GetDailyWinLossCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetAllProperties_WhenProvided()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var whiteLabels = "Label1,Label2";
        var username = "testuser";

        // Act
        var criteria = new GetDailyWinLossCriteria(custId, fromDate, toDate, whiteLabels, username);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(whiteLabels, criteria.WhiteLabelList);
        Assert.Equal(username, criteria.Username);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetDailyWinLossCriteria(1, DateTime.Now, DateTime.Now, "", "");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WhiteLabelWinLoss", result);
    }
}