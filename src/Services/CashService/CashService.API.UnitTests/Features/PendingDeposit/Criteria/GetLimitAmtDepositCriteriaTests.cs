using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class GetLimitAmtDepositCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var criteria = new GetLimitAmtDepositCriteria();

        // Assert
        Assert.Equal(0, criteria.CustId);
        Assert.Null(criteria.AccumCreditReturn);
        Assert.Equal(0, criteria.CustDbGroupId);
        Assert.Equal(0, criteria.AgentAvailableCredit);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var criteria = new GetLimitAmtDepositCriteria();
        var custId = 1;
        var accumCreditReturn = 100.5m;
        var custDbGroupId = 2;
        var agentAvailableCredit = 200.75m;

        // Act
        criteria.CustId = custId;
        criteria.AccumCreditReturn = accumCreditReturn;
        criteria.CustDbGroupId = custDbGroupId;
        criteria.AgentAvailableCredit = agentAvailableCredit;

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(accumCreditReturn, criteria.AccumCreditReturn);
        Assert.Equal(custDbGroupId, criteria.CustDbGroupId);
        Assert.Equal(agentAvailableCredit, criteria.AgentAvailableCredit);
    }

    [Theory]
    [InlineData(1, "CustDb_1_Age_CashDeposit_LimitAmtSel")]
    [InlineData(5, "CustDb_5_Age_CashDeposit_LimitAmtSel")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int custDbGroupId, string expectedKey)
    {
        // Arrange
        var criteria = new GetLimitAmtDepositCriteria { CustDbGroupId = custDbGroupId };

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
        var criteria = new GetLimitAmtDepositCriteria { CustId = custId };

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}