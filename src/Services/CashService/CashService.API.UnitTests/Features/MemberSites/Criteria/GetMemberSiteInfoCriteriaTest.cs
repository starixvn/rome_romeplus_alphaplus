using AlphaPlus.CashService.API.Features.MemberSites.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Criteria;

//
public class GetMemberSiteInfoCriteriaTests
{
    [Fact]
    public void Constructor_WhenValidRequestProvided_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var custId = 1;

        // Act
        var criteria = new GetMemberSiteInfoCriteria(custId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetMemberSiteInfoCriteria(1);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_GeneralSetting", settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateCustIdCorrectly(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetMemberSiteInfoCriteria(custId);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }
}