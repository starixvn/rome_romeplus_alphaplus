using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Withdrawal;

[Trait("Category", "Withdrawal")]
public class UpdateWithdrawalTransStatusCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        const int agentId = 123;
        const int custId = 456;
        const long transId = 789;
        const byte statusId = 1;
        const short siteDBGroupId = 10;
        string providerTransId = "PROV123";
        const string ip = "192.168.1.1";

        // Act
        var criteria = new UpdateWithdrawalTransStatusCriteria(
            agentId, custId, transId, statusId, siteDBGroupId, providerTransId, ip);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(statusId, criteria.StatusId);
        Assert.Equal(siteDBGroupId, criteria.SiteDBGroupId);
        Assert.Equal(providerTransId, criteria.ProviderTransId);
        Assert.Equal(ip, criteria.IP);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SP_Name()
    {
        // Arrange
        const short siteDBGroupId = 5;
        var criteria = new UpdateWithdrawalTransStatusCriteria(
            1, 1, 1, 1, siteDBGroupId, null, "127.0.0.1");
        var expectedSpName = SPNameHelper.SiteDBPrefix("API_CashWithdrawal_UpdateTransStatus", siteDBGroupId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSpName, result);
    }

    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(0, 1, 1, false)]
    [InlineData(1, 0, 1, false)]
    [InlineData(1, 1, 0, false)]
    [InlineData(0, 0, 0, false)]
    public void IsValid_Should_Return_Expected_Result(int agentId, int custId, long transId, bool expected)
    {
        // Arrange
        var criteria = new UpdateWithdrawalTransStatusCriteria(
            agentId, custId, transId, 1, 5, null, "127.0.0.1");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}