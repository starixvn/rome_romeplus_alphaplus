using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Withdrawal;

[Trait("Category", "Withdrawal")]
public class ManualWithdrawalTransactionCriteriaTest
{
    private const int ValidCustId = 123;
    private const int ValidAgentId = 456;
    private const short ValidSiteDBGroupId = 1;
    private const byte ValidTransactionTypeId = 2;
    private const decimal ValidMinAmt = 10.0m;
    private const decimal ValidMaxAmt = 1000.0m;
    private const short ValidDailyWithdrawalCnt = 5;
    private const string ValidPaymentTo = "Bank Account";
    private const decimal ValidAmount = 500.0m;
    private const string ValidIP = "192.168.1.1";

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange & Act
        var criteria = new ManualWithdrawalTransactionCriteria(
            custId: ValidCustId,
            agentId: ValidAgentId,
            siteDBGroupId: ValidSiteDBGroupId,
            transactionTypeId: ValidTransactionTypeId,
            minAmt: ValidMinAmt,
            maxAmt: ValidMaxAmt,
            dailyWithdrawalCnt_Setting: ValidDailyWithdrawalCnt,
            paymentTo: ValidPaymentTo,
            amount: ValidAmount,
            ip: ValidIP);

        // Assert
        Assert.Equal(ValidCustId, criteria.CustId);
        Assert.Equal(ValidAgentId, criteria.AgentId);
        Assert.Equal(ValidSiteDBGroupId, criteria.SiteDBGroupId);
        Assert.Equal(ValidTransactionTypeId, criteria.TransactionTypeId);
        Assert.Equal(ValidMinAmt, criteria.MinAmt);
        Assert.Equal(ValidMaxAmt, criteria.MaxAmt);
        Assert.Equal(ValidDailyWithdrawalCnt, criteria.DailyWithdrawalCnt_Setting);
        Assert.Equal(ValidPaymentTo, criteria.PaymentTo);
        Assert.Equal(ValidAmount, criteria.Amount);
        Assert.Equal(ValidIP, criteria.IP);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SPName()
    {
        // Arrange
        var criteria = CreateValidCriteria();
        var expectedSpName = SPNameHelper.SiteDBPrefix("API_CashWithdrawal_AddTransaction", ValidSiteDBGroupId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSpName, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_For_Valid_Criteria()
    {
        // Arrange
        var criteria = CreateValidCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 456, "Bank Account", 500.0)]
    [InlineData(123, 0, "Bank Account", 500.0)]
    [InlineData(123, 456, "", 500.0)]
    [InlineData(123, 456, null, 500.0)]
    [InlineData(123, 456, "Bank Account", 0)]
    [InlineData(123, 456, "Bank Account", -1)]
    public void IsValid_Should_Return_False_For_Invalid_Criteria(int custId, int agentId, string paymentTo, decimal amount)
    {
        // Arrange
        var criteria = new ManualWithdrawalTransactionCriteria(
            custId: custId,
            agentId: agentId,
            siteDBGroupId: ValidSiteDBGroupId,
            transactionTypeId: ValidTransactionTypeId,
            minAmt: ValidMinAmt,
            maxAmt: ValidMaxAmt,
            dailyWithdrawalCnt_Setting: ValidDailyWithdrawalCnt,
            paymentTo: paymentTo,
            amount: amount,
            ip: ValidIP);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OutputParameters_Should_Be_Set_And_Retrieved_Correctly()
    {
        // Arrange
        var criteria = CreateValidCriteria();
        const int expectedErrCode = 123;
        const string expectedErrMsg = "Test error message";
        const string expectedErrExtraInfo = "Extra info";
        const long expectedTransId = 789012345;

        // Act
        criteria.ErrCode = expectedErrCode;
        criteria.ErrMsg = expectedErrMsg;
        criteria.ErrExtraInfo = expectedErrExtraInfo;
        criteria.TransId = expectedTransId;

        // Assert
        Assert.Equal(expectedErrCode, criteria.ErrCode);
        Assert.Equal(expectedErrMsg, criteria.ErrMsg);
        Assert.Equal(expectedErrExtraInfo, criteria.ErrExtraInfo);
        Assert.Equal(expectedTransId, criteria.TransId);
    }

    private static ManualWithdrawalTransactionCriteria CreateValidCriteria()
    {
        return new ManualWithdrawalTransactionCriteria(
            custId: ValidCustId,
            agentId: ValidAgentId,
            siteDBGroupId: ValidSiteDBGroupId,
            transactionTypeId: ValidTransactionTypeId,
            minAmt: ValidMinAmt,
            maxAmt: ValidMaxAmt,
            dailyWithdrawalCnt_Setting: ValidDailyWithdrawalCnt,
            paymentTo: ValidPaymentTo,
            amount: ValidAmount,
            ip: ValidIP);
    }
}