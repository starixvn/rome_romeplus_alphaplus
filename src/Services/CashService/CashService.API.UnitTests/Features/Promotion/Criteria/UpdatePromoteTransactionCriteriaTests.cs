using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class UpdatePromoteTransactionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 1;
        var agentId = 2;
        var subAccId = 3;
        long transId = 123456789;
        var statusId = 4;
        var newAmount = 100.50m;
        var agentGivenCredit = 200.75m;
        var creditMembersGivenCredit = 300.25m;
        var ip = "192.168.1.1";
        var remark = "Test remark";
        var cashAccumCreditTbl = "CashAccumCredit";
        var siteDbGroupId = 5;

        // Act
        var criteria = new UpdatePromoteTransactionCriteria
        {
            CustId = custId,
            AgentId = agentId,
            SubAccId = subAccId,
            TransId = transId,
            StatusId = statusId,
            NewAmount = newAmount,
            AgentGivenCredit = agentGivenCredit,
            CreditMembersGivenCredit = creditMembersGivenCredit,
            IP = ip,
            Remark = remark,
            CashAccumCreditTbl = cashAccumCreditTbl,
            SiteDbGroupId = siteDbGroupId
        };

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(statusId, criteria.StatusId);
        Assert.Equal(newAmount, criteria.NewAmount);
        Assert.Equal(agentGivenCredit, criteria.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, criteria.CreditMembersGivenCredit);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(remark, criteria.Remark);
        Assert.Equal(cashAccumCreditTbl, criteria.CashAccumCreditTbl);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new UpdatePromoteTransactionCriteria
        {
            SiteDbGroupId = 10
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDb_10_Age_CashPromotion_UpdateTransStatus", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Arrange
        var criteria = new UpdatePromoteTransactionCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Return_True_For_Any_CustId(int custId)
    {
        // Arrange
        var criteria = new UpdatePromoteTransactionCriteria
        {
            CustId = custId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(100.50)]
    [InlineData(-50.25)]
    public void Decimal_Properties_Should_Accept_Various_Values(decimal value)
    {
        // Arrange & Act
        var criteria = new UpdatePromoteTransactionCriteria
        {
            NewAmount = value,
            AgentGivenCredit = value,
            CreditMembersGivenCredit = value
        };

        // Assert
        Assert.Equal(value, criteria.NewAmount);
        Assert.Equal(value, criteria.AgentGivenCredit);
        Assert.Equal(value, criteria.CreditMembersGivenCredit);
    }

    [Fact]
    public void Properties_Should_Accept_Null_Values()
    {
        // Arrange & Act
        var criteria = new UpdatePromoteTransactionCriteria
        {
            IP = null,
            Remark = null,
            CashAccumCreditTbl = null
        };

        // Assert
        Assert.Null(criteria.IP);
        Assert.Null(criteria.Remark);
        Assert.Null(criteria.CashAccumCreditTbl);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Strings()
    {
        // Arrange & Act
        var criteria = new UpdatePromoteTransactionCriteria
        {
            IP = "",
            Remark = "",
            CashAccumCreditTbl = ""
        };

        // Assert
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.Remark);
        Assert.Equal("", criteria.CashAccumCreditTbl);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(123456789L)]
    [InlineData(-1L)]
    public void TransId_Should_Accept_Various_Values(long transId)
    {
        // Arrange & Act
        var criteria = new UpdatePromoteTransactionCriteria
        {
            TransId = transId
        };

        // Assert
        Assert.Equal(transId, criteria.TransId);
    }
}
