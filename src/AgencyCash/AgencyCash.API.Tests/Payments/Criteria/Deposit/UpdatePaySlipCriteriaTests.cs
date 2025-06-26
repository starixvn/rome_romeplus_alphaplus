using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Deposit;

[Trait("Category", "Deposit")]
public class UpdatePaySlipCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithProvidedValues()
    {
        // Arrange
        const int agentId = 123;
        const int custId = 456;
        const long transId = 789012;
        var paySlip = new byte[] { 1, 2, 3, 4, 5 };
        const string ip = "192.168.1.1";
        const short siteDBGroupId = 2;

        // Act
        var criteria = new UpdatePaySlipCriteria(
            agentId,
            custId,
            transId,
            paySlip,
            ip,
            siteDBGroupId);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(paySlip, criteria.PaySlip);
        Assert.Equal(ip, criteria.IP);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectSPName()
    {
        // Arrange
        const short siteDBGroupId = 3;
        var criteria = new UpdatePaySlipCriteria(1, 2, 3, Array.Empty<byte>(), "127.0.0.1", siteDBGroupId);
        var expectedSPName = SPNameHelper.SiteDBPrefix("API_CashDeposit_Payslip_Ins", siteDBGroupId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSPName, result);
    }

    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(0, 1, 1, false)]
    [InlineData(1, 0, 1, false)]
    [InlineData(1, 1, 0, false)]
    [InlineData(0, 0, 0, false)]
    public void IsValid_ShouldReturnExpectedResult_BasedOnIds(int agentId, int custId, long transId, bool expected)
    {
        // Arrange
        var criteria = new UpdatePaySlipCriteria(
            agentId,
            custId,
            transId,
            Array.Empty<byte>(),
            "127.0.0.1",
            1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ErrorProperties_ShouldBeSettable()
    {
        // Arrange
        var criteria = new UpdatePaySlipCriteria(1, 2, 3, Array.Empty<byte>(), "127.0.0.1", 1);
        const int expectedErrCode = 400;
        const string expectedErrMsg = "Bad Request";
        const string expectedErrExtraInfo = "Extra error information";

        // Act
        criteria.ErrCode = expectedErrCode;
        criteria.ErrMsg = expectedErrMsg;
        criteria.ErrExtraInfo = expectedErrExtraInfo;

        // Assert
        Assert.Equal(expectedErrCode, criteria.ErrCode);
        Assert.Equal(expectedErrMsg, criteria.ErrMsg);
        Assert.Equal(expectedErrExtraInfo, criteria.ErrExtraInfo);
    }
}