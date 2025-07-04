using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.PendingDeposit.Criteria;

public class CreateManualDepositCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesToDefaultValues()
    {
        // Act
        var command = new CreateManualDepositCommand();

        // Assert
        Assert.Equal(0, command.CustId);
        Assert.Equal(0, command.UplineId);
        Assert.Equal(0, command.SubAccId);
        Assert.Equal(0, command.AgentGivenCredit);
        Assert.Equal(0, command.CreditMembers_GivenCredit);
        Assert.Null(command.PaymentFrom);
        Assert.Null(command.PaymentTo);
        Assert.Equal(0, command.Amount);
        Assert.Null(command.PaySlip);
        Assert.Null(command.Ip);
        Assert.Null(command.Remark);
        Assert.Null(command.CashAccumCreditTbl);
        Assert.Equal(0, command.ErrCode);
        Assert.Null(command.ErrMsg);
        Assert.Equal(0, command.SiteDbGroupId);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var command = new CreateManualDepositCommand();
        var custId = 1;
        var uplineId = 2;
        var subAccId = 3;
        var agentGivenCredit = 100.50m;
        var creditMembersGivenCredit = 50.25m;
        var paymentFrom = "Bank A";
        var paymentTo = "Account B";
        var amount = 200.00m;
        var paySlip = new byte[] { 1, 2, 3 };
        var ip = "127.0.0.1";
        var remark = "Test Deposit";
        var cashAccumCreditTbl = "tbl_Accum";
        var errCode = -1;
        var errMsg = "Error";
        var siteDbGroupId = 4;

        // Act
        command.CustId = custId;
        command.UplineId = uplineId;
        command.SubAccId = subAccId;
        command.AgentGivenCredit = agentGivenCredit;
        command.CreditMembers_GivenCredit = creditMembersGivenCredit;
        command.PaymentFrom = paymentFrom;
        command.PaymentTo = paymentTo;
        command.Amount = amount;
        command.PaySlip = paySlip;
        command.Ip = ip;
        command.Remark = remark;
        command.CashAccumCreditTbl = cashAccumCreditTbl;
        command.ErrCode = errCode;
        command.ErrMsg = errMsg;
        command.SiteDbGroupId = siteDbGroupId;

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(uplineId, command.UplineId);
        Assert.Equal(subAccId, command.SubAccId);
        Assert.Equal(agentGivenCredit, command.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, command.CreditMembers_GivenCredit);
        Assert.Equal(paymentFrom, command.PaymentFrom);
        Assert.Equal(paymentTo, command.PaymentTo);
        Assert.Equal(amount, command.Amount);
        Assert.Equal(paySlip, command.PaySlip);
        Assert.Equal(ip, command.Ip);
        Assert.Equal(remark, command.Remark);
        Assert.Equal(cashAccumCreditTbl, command.CashAccumCreditTbl);
        Assert.Equal(errCode, command.ErrCode);
        Assert.Equal(errMsg, command.ErrMsg);
        Assert.Equal(siteDbGroupId, command.SiteDbGroupId);
    }

    [Theory]
    [InlineData(1, "SiteDb_1_Age_CashDeposit_ManualAdd")]
    [InlineData(5, "SiteDb_5_Age_CashDeposit_ManualAdd")]
    public void GetSettingKey_ShouldReturnCorrectFormat(int siteDbGroupId, string expectedKey)
    {
        // Arrange
        var command = new CreateManualDepositCommand { SiteDbGroupId = siteDbGroupId };

        // Act
        var settingKey = command.GetSettingKey();

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
        var command = new CreateManualDepositCommand { CustId = custId };

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }
}
