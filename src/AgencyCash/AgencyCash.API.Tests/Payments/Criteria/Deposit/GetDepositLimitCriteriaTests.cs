using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Deposit;

[Trait("Category", "Deposit")]
public class GetDepositLimitCriteriaTests
{
    [Fact]
    public void Constructor_WithAccumCreditReturn_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        const int custId = 1001;
        const byte paymentType = 2;
        const short providerId = 5;
        const short paymentMethodId = 10;
        const decimal accumCreditReturn = 100.50m;
        const short custDBGroupId = 3;

        // Act
        var criteria = new GetDepositLimitCriteria(
            custId,
            paymentType,
            providerId,
            paymentMethodId,
            accumCreditReturn,
            custDBGroupId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(paymentType, criteria.PaymentType);
        Assert.Equal(providerId, criteria.ProviderId);
        Assert.Equal(paymentMethodId, criteria.PaymentMethodId);
        Assert.Equal(accumCreditReturn, criteria.AccumCreditReturn);
        Assert.Equal(custDBGroupId, criteria.CustDBGroupId);
        Assert.Null(criteria.AgentId);
        Assert.True(criteria.IsMemberSite);
    }

    [Fact]
    public void Constructor_WithAgentId_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        const int agentId = 500;
        const int custId = 1001;
        const byte paymentType = 2;
        const short providerId = 5;
        const short paymentMethodId = 10;
        const short custDBGroupId = 3;

        // Act
        var criteria = new GetDepositLimitCriteria(
            agentId,
            custId,
            paymentType,
            providerId,
            paymentMethodId,
            custDBGroupId);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(paymentType, criteria.PaymentType);
        Assert.Equal(providerId, criteria.ProviderId);
        Assert.Equal(paymentMethodId, criteria.PaymentMethodId);
        Assert.Equal(custDBGroupId, criteria.CustDBGroupId);
        Assert.Equal(0, criteria.AccumCreditReturn);
        Assert.True(criteria.IsMemberSite);
    }

    [Fact]
    public void Constructor_WithDefaultValues_ShouldInitializeCorrectly()
    {
        // Arrange
        const int agentId = 500;
        const int custId = 1001;
        const byte paymentType = 2;

        // Act
        var criteria = new GetDepositLimitCriteria(agentId, custId, paymentType);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(paymentType, criteria.PaymentType);
        Assert.Equal(0, criteria.ProviderId);
        Assert.Equal(0, criteria.PaymentMethodId);
        Assert.Equal(0, criteria.CustDBGroupId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectSPName()
    {
        // Arrange
        const short custDBGroupId = 5;
        var criteria = new GetDepositLimitCriteria(1001, 2, 5, 10, 100.50m, custDBGroupId);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(SPNameHelper.CustDBPrefix("Age_CashDeposit_LimitAmtSel", custDBGroupId), settingKey);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(1, true)]
    [InlineData(1001, true)]
    public void IsValid_ShouldValidateCustId(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetDepositLimitCriteria(custId, 2, 5, 10, 100.50m, 3);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void AmountProperties_ShouldBeSettable()
    {
        // Arrange
        var criteria = new GetDepositLimitCriteria(1001, 2, 5, 10, 100.50m, 3);

        // Act
        criteria.MinAmt = 50.0m;
        criteria.MaxAmt = 1000.0m;
        criteria.AgentAvailableCredit = 5000.0m;
        criteria.AgentGivenCredit = 3000.0m;
        criteria.CreditMembers_GivenCredit = 2000.0m;

        // Assert
        Assert.Equal(50.0m, criteria.MinAmt);
        Assert.Equal(1000.0m, criteria.MaxAmt);
        Assert.Equal(5000.0m, criteria.AgentAvailableCredit);
        Assert.Equal(3000.0m, criteria.AgentGivenCredit);
        Assert.Equal(2000.0m, criteria.CreditMembers_GivenCredit);
    }
}
