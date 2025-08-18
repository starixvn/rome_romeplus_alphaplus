using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "Promotion")]
public class GetPromotionReportCriteriaTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var custId = 12345;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        byte promotionType = 2;
        var statusId = true;

        // Act
        var criteria = new GetPromotionReportCriteria(
            custId,
            fromDate,
            toDate,
            promotionType,
            statusId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(promotionType, criteria.PromotionType);
        Assert.Equal(statusId, criteria.StatusId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetPromotionReportCriteria(1, DateTime.Now, DateTime.Now, null, null);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashPromotion_PromotionReport", settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetPromotionReportCriteria(
            custId,
            DateTime.Now,
            DateTime.Now,
            null,
            null);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void Constructor_Should_Initialize_With_Nullable_Parameters()
    {
        // Arrange
        var custId = 12345;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);

        // Act
        var criteria = new GetPromotionReportCriteria(
            custId,
            fromDate,
            toDate,
            null,
            null);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Null(criteria.PromotionType);
        Assert.Null(criteria.StatusId);
    }
}