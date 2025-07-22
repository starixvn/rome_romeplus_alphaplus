using AgencyCash.API.Payments.Criteria;

namespace AgencyCash.API.Tests.Payments.Criteria;
public class UpdateTransProviderInfoCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenInitialized()
    {
        // Arrange
        const int agentId = 123;
        const int custId = 456;
        const long transId = 789012;
        const byte transType = 1;
        const string providerTransId = "PROVIDER-123";
        const string ip = "192.168.1.1";
        const short siteDBGroupId = 2;

        // Act
        var criteria = new UpdateTransProviderInfoCriteria(
            agentId, custId, transId, transType, providerTransId, ip, siteDBGroupId);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(transType, criteria.TransType);
        Assert.Equal(providerTransId, criteria.ProviderTransId);
        Assert.Equal(ip, criteria.IP);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectSPName()
    {
        // Arrange
        const short siteDBGroupId = 3;
        var criteria = new UpdateTransProviderInfoCriteria(
            1, 1, 1, 1, "test", "127.0.0.1", siteDBGroupId);
        var expectedKey = "SiteDB3_API_CashTransaction_ProviderInfo_Upd";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(0, 1, 1, false)]
    [InlineData(1, 0, 1, false)]
    [InlineData(1, 1, 0, false)]
    [InlineData(0, 0, 0, false)]
    public void IsValid_ShouldReturnExpectedResult(int agentId, int custId, long transId, bool expectedResult)
    {
        // Arrange
        var criteria = new UpdateTransProviderInfoCriteria(
            agentId, custId, transId, 1, "test", "127.0.0.1", 1);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void ErrorProperties_ShouldBeSettable()
    {
        // Arrange
        var criteria = new UpdateTransProviderInfoCriteria(
            1, 1, 1, 1, "test", "127.0.0.1", 1);
        const int errCode = 500;
        const string errMsg = "Test error message";
        const string errExtraInfo = "Extra error details";

        // Act
        criteria.ErrCode = errCode;
        criteria.ErrMsg = errMsg;
        criteria.ErrExtraInfo = errExtraInfo;

        // Assert
        Assert.Equal(errCode, criteria.ErrCode);
        Assert.Equal(errMsg, criteria.ErrMsg);
        Assert.Equal(errExtraInfo, criteria.ErrExtraInfo);
    }
}
