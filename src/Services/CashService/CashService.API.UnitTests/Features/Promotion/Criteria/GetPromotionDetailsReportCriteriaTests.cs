using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class GetPromotionDetailsReportCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 1;
        var promotionId = 2;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        byte? statusId = 3;

        // Act
        var criteria = new GetPromotionDetailsReportCriteria(custId, promotionId, fromDate, toDate, statusId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(statusId, criteria.StatusId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetPromotionDetailsReportCriteria(1, 2, DateTime.Now, DateTime.Now, 3);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashPromotion_PromotionDetailsReport", result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, int promotionId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetPromotionDetailsReportCriteria(
            custId,
            promotionId,
            DateTime.Now,
            DateTime.Now,
            1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_Required_Properties_Are_Valid()
    {
        // Arrange
        var criteria = new GetPromotionDetailsReportCriteria(1, 2, DateTime.Now, DateTime.Now, null);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Constructor_Should_Accept_Null_StatusId()
    {
        // Arrange & Act
        var criteria = new GetPromotionDetailsReportCriteria(1, 2, DateTime.Now, DateTime.Now, null);

        // Assert
        Assert.Null(criteria.StatusId);
    }
}