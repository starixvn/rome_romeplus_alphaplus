using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class GetMemberSettingDepositCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var criteria = new GetMemberSettingDepositCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
        Assert.Equal(0, criteria.CustDbGroupId);
        Assert.Equal(0, criteria.AgentGivenCredit);
        Assert.Equal(0, criteria.CreditMembers_GivenCredit);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var criteria = new GetMemberSettingDepositCriteria();
        var custId = 1;
        var custDbGroupId = 2;
        var agentGivenCredit = 100.5m;
        var creditMembersGivenCredit = 50.25m;

        // Act
        criteria.CustId = custId;
        criteria.CustDbGroupId = custDbGroupId;
        criteria.AgentGivenCredit = agentGivenCredit;
        criteria.CreditMembers_GivenCredit = creditMembersGivenCredit;

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(custDbGroupId, criteria.CustDbGroupId);
        Assert.Equal(agentGivenCredit, criteria.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, criteria.CreditMembers_GivenCredit);
    }

    [Theory]
    [InlineData(1, "CustDb_1_Age_CashDeposit_MemberSettingSel")]
    [InlineData(5, "CustDb_5_Age_CashDeposit_MemberSettingSel")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int custDbGroupId, string expectedKey)
    {
        // Arrange
        var criteria = new GetMemberSettingDepositCriteria { CustDbGroupId = custDbGroupId };

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldValidateCustId(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetMemberSettingDepositCriteria { CustId = custId };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
