using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Deposit;

[Trait("Category", "Deposit")]
public class AddTransactionDepositCriteriaTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int agentId = 1;
        int custId = 2;
        short siteDBGroupId = 3;
        decimal agentGivenCredit = 100.0m;
        decimal creditMembersGivenCredit = 200.0m;
        decimal minAmount = 10.0m;
        decimal maxAmount = 1000.0m;
        byte transactionTypeId = 1;
        byte paymentType = 2;
        short paymentMethodId = 3;
        string paymentFrom = "source";
        string paymentTo = "destination";
        decimal amount = 500.0m;
        decimal confirmAmount = 500.0m;
        byte matchingMode = 1;
        short providerId = 1;
        string ip = "127.0.0.1";
        string remark = "test remark";

        // Act
        var criteria = new AddTransactionDepositCriteria(
            agentId, custId, siteDBGroupId, agentGivenCredit,
            creditMembersGivenCredit, minAmount, maxAmount,
            transactionTypeId, paymentType, paymentMethodId,
            paymentFrom, paymentTo, amount, confirmAmount,
            matchingMode, providerId, ip, remark);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(siteDBGroupId, criteria.SiteDBGroupId);
        Assert.Equal(agentGivenCredit, criteria.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, criteria.CreditMembers_GivenCredit);
        Assert.Equal(minAmount, criteria.MinAmt);
        Assert.Equal(maxAmount, criteria.MaxAmt);
        Assert.Equal(transactionTypeId, criteria.TransactionTypeId);
        Assert.Equal(paymentType, criteria.PaymentType);
        Assert.Equal(paymentMethodId, criteria.PaymentMethodId);
        Assert.Equal(paymentFrom, criteria.PaymentFrom);
        Assert.Equal(paymentTo, criteria.PaymentTo);
        Assert.Equal(amount, criteria.Amount);
        Assert.Equal(confirmAmount, criteria.UserInput_Amount);
        Assert.Equal(matchingMode, criteria.MatchingMode);
        Assert.Equal(providerId, criteria.ProviderId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(remark, criteria.Remark);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SPName()
    {
        // Arrange
        short siteDBGroupId = 3;
        var criteria = new AddTransactionDepositCriteria(
            1, 2, siteDBGroupId, 100, 200, 10, 1000,
            1, 2, 3, "from", "to", 500);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        var expected = SPNameHelper.SiteDBPrefix("API_CashDeposit_AddTransaction", siteDBGroupId);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_CustId_Should_Return_Expected_Result(int custId, bool expected)
    {
        // Arrange
        var criteria = new AddTransactionDepositCriteria(
            1, custId, 3, 100, 200, 10, 1000,
            1, 2, 3, "from", "to", 500);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Agent_Should_Return_Expected_Result(int agent, bool expected)
    {
        // Arrange
        var criteria = new AddTransactionDepositCriteria(
            agent, 1, 3, 100, 200, 10, 1000,
            1, 2, 3, "from", "to", 500);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(500, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Amount_Should_Return_Expected_Result(decimal amount, bool expected)
    {
        // Arrange
        var criteria = new AddTransactionDepositCriteria(
            1, 1, 3, 100, 200, 10, 1000,
            1, 2, 3, "from", "to", amount);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}