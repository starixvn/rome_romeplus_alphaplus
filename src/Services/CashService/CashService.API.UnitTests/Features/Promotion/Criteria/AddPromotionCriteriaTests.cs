using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class AddPromotionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var promotionType = 1;
        var enabled = true;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var promotionDetails = "Test Details";
        var promotionContents = "Test Contents";
        var createdBy = 123;
        var ip = "192.168.1.1";
        var language = "en";
        var siteDbGroupId = 1;

        // Act
        var criteria = new AddPromotionCriteria
        {
            PromotionType = promotionType,
            Enabled = enabled,
            FromDate = fromDate,
            ToDate = toDate,
            PromotionDetails = promotionDetails,
            PromotionContents = promotionContents,
            CreatedBy = createdBy,
            IP = ip,
            Language = language,
            SiteDbGroupId = siteDbGroupId
        };

        // Assert
        Assert.Equal(promotionType, criteria.PromotionType);
        Assert.Equal(enabled, criteria.Enabled);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(promotionDetails, criteria.PromotionDetails);
        Assert.Equal(promotionContents, criteria.PromotionContents);
        Assert.Equal(createdBy, criteria.CreatedBy);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(language, criteria.Language);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new AddPromotionCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_CashPromotionIns", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Arrange
        var criteria = new AddPromotionCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    public void IsValid_Should_Return_True_For_Any_PromotionType(int promotionType, bool expectedResult)
    {
        // Arrange
        var criteria = new AddPromotionCriteria
        {
            PromotionType = promotionType
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsValid_Should_Return_True_For_Any_Enabled_Value(bool enabled)
    {
        // Arrange
        var criteria = new AddPromotionCriteria
        {
            Enabled = enabled
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Properties_Should_Accept_Null_Values()
    {
        // Arrange & Act
        var criteria = new AddPromotionCriteria
        {
            PromotionDetails = null,
            PromotionContents = null,
            IP = null,
            Language = null
        };

        // Assert
        Assert.Null(criteria.PromotionDetails);
        Assert.Null(criteria.PromotionContents);
        Assert.Null(criteria.IP);
        Assert.Null(criteria.Language);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Strings()
    {
        // Arrange & Act
        var criteria = new AddPromotionCriteria
        {
            PromotionDetails = "",
            PromotionContents = "",
            IP = "",
            Language = ""
        };

        // Assert
        Assert.Equal("", criteria.PromotionDetails);
        Assert.Equal("", criteria.PromotionContents);
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.Language);
    }
}
