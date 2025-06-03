using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingWithdrawal.Criteria;

public class GetMemberSettingWithdrawCriteriaTests
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Act
        var criteria = new GetMemberSettingWithdrawCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var criteria = new GetMemberSettingWithdrawCriteria();
        var custId = 123;

        // Act
        criteria.CustId = custId;

        // Assert
        Assert.Equal(custId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetMemberSettingWithdrawCriteria();

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashWithdrawal_MemberSettingSel", settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateCustIdCorrectly(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetMemberSettingWithdrawCriteria
        {
            CustId = custId
        };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnConstantValue_RegardlessOfCustId()
    {
        // Arrange
        var criteria1 = new GetMemberSettingWithdrawCriteria { CustId = 1 };
        var criteria2 = new GetMemberSettingWithdrawCriteria { CustId = 999 };

        // Act
        var settingKey1 = criteria1.GetSettingKey();
        var settingKey2 = criteria2.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashWithdrawal_MemberSettingSel", settingKey1);
        Assert.Equal("Age_CashWithdrawal_MemberSettingSel", settingKey2);
        Assert.Equal(settingKey1, settingKey2);
    }

    [Fact]
    public void CustId_WhenSetToLargeValue_ShouldStoreCorrectly()
    {
        // Arrange
        var criteria = new GetMemberSettingWithdrawCriteria();
        var largeCustId = int.MaxValue;

        // Act
        criteria.CustId = largeCustId;

        // Assert
        Assert.Equal(largeCustId, criteria.CustId);
        Assert.True(criteria.IsValid());
    }

    [Fact]
    public void CustId_WhenSetToMinimumValidValue_ShouldBeValid()
    {
        // Arrange
        var criteria = new GetMemberSettingWithdrawCriteria();

        // Act
        criteria.CustId = 1;

        // Assert
        Assert.Equal(1, criteria.CustId);
        Assert.True(criteria.IsValid());
    }
}
