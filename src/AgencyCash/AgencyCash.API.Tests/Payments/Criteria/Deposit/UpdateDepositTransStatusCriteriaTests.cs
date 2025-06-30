using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Deposit;

[Trait("Category", "Deposit")]
public class UpdateDepositTransStatusCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        int agentId = 12345;
        int custId = 67890;
        long transId = 1000001;
        byte statusId = 2;
        short siteDBGroupId = 123;
        decimal agentGivenCredit = 100.50m;
        decimal creditMembersGivenCredit = 200.75m;
        string providerTransId = "PROV123";
        string ip = "192.168.1.1";

        // Act
        var criteria = new UpdateDepositTransStatusCriteria(
            agentId,
            custId,
            transId,
            statusId,
            siteDBGroupId,
            agentGivenCredit,
            creditMembersGivenCredit,
            providerTransId,
            ip);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(statusId, criteria.StatusId);
        Assert.Equal(siteDBGroupId, criteria.SiteDBGroupId);
        Assert.Equal(agentGivenCredit, criteria.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, criteria.CreditMembers_GivenCredit);
        Assert.Equal(providerTransId, criteria.ProviderTransId);
        Assert.Equal(ip, criteria.IP);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectStoredProcedureName()
    {
        // Arrange
        short siteDBGroupId = 42;
        var criteria = new UpdateDepositTransStatusCriteria(1, 2, 3, 4, siteDBGroupId, 100, 200, null, "127.0.0.1");
        string expected = SPNameHelper.SiteDBPrefix("API_CashDeposit_UpdateTransStatus", siteDBGroupId);

        // Act
        string actual = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(0, 1, 1, false)]
    [InlineData(1, 0, 1, false)]
    [InlineData(1, 1, 0, false)]
    [InlineData(0, 0, 0, false)]
    public void IsValid_ShouldValidateCorrectly(int agentId, int custId, long transId, bool expected)
    {
        // Arrange
        var criteria = new UpdateDepositTransStatusCriteria(
            agentId,
            custId,
            transId,
            1,
            1,
            100,
            100,
            null,
            "127.0.0.1");

        // Act
        bool isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expected, isValid);
    }

    [Fact]
    public void ErrorProperties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var criteria = new UpdateDepositTransStatusCriteria(1, 2, 3, 4, 5, 100, 200, null, "127.0.0.1");
        int errCode = 500;
        string errMsg = "Test Error Message";
        string errExtraInfo = "Extra error details";
        string cashAccumCreditTbl = "CashAccumTable";

        // Act
        criteria.ErrCode = errCode;
        criteria.ErrMsg = errMsg;
        criteria.ErrExtraInfo = errExtraInfo;
        criteria.CashAccumCreditTbl = cashAccumCreditTbl;

        // Assert
        Assert.Equal(errCode, criteria.ErrCode);
        Assert.Equal(errMsg, criteria.ErrMsg);
        Assert.Equal(errExtraInfo, criteria.ErrExtraInfo);
        Assert.Equal(cashAccumCreditTbl, criteria.CashAccumCreditTbl);
    }
}
