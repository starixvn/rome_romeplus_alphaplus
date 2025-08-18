using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class AddBonusCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 1;
        var uplineId = 2;
        var subAccId = 3;
        var agentGivenCredit = 100.50m;
        var creditMembersGivenCredit = 200.75m;
        var exchangeRate = 1.25m;
        var amount = 500.00m;
        var ip = "192.168.1.1";
        var remark = "Test bonus";
        var siteDbGroupId = 1;
        var custDbGroupId = 2;
        var cashAccumCreditTbl = "CashAccumCredit";

        // Act
        var criteria = new AddBonusCriteria
        {
            CustId = custId,
            UplineId = uplineId,
            SubAccId = subAccId,
            AgentGivenCredit = agentGivenCredit,
            CreditMembers_GivenCredit = creditMembersGivenCredit,
            ExchangeRate = exchangeRate,
            Amount = amount,
            IP = ip,
            Remark = remark,
            SiteDbGroupId = siteDbGroupId,
            CustDbGroupId = custDbGroupId,
            CashAccumCreditTbl = cashAccumCreditTbl
        };

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(uplineId, criteria.UplineId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(agentGivenCredit, criteria.AgentGivenCredit);
        Assert.Equal(creditMembersGivenCredit, criteria.CreditMembers_GivenCredit);
        Assert.Equal(exchangeRate, criteria.ExchangeRate);
        Assert.Equal(amount, criteria.Amount);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(remark, criteria.Remark);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
        Assert.Equal(custDbGroupId, criteria.CustDbGroupId);
        Assert.Equal(cashAccumCreditTbl, criteria.CashAccumCreditTbl);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new AddBonusCriteria
        {
            SiteDbGroupId = 5
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDb_5_Age_CashPromotion_ManualAddBonus", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Arrange
        var criteria = new AddBonusCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    public void IsValid_Should_Return_True_For_Any_CustId(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new AddBonusCriteria
        {
            CustId = custId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Properties_Should_Accept_Null_Values()
    {
        // Arrange & Act
        var criteria = new AddBonusCriteria
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
        var criteria = new AddBonusCriteria
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
    [InlineData(0.0)]
    [InlineData(100.50)]
    [InlineData(-50.25)]
    public void Decimal_Properties_Should_Accept_Various_Values(decimal value)
    {
        // Arrange & Act
        var criteria = new AddBonusCriteria
        {
            AgentGivenCredit = value,
            CreditMembers_GivenCredit = value,
            ExchangeRate = value,
            Amount = value
        };

        // Assert
        Assert.Equal(value, criteria.AgentGivenCredit);
        Assert.Equal(value, criteria.CreditMembers_GivenCredit);
        Assert.Equal(value, criteria.ExchangeRate);
        Assert.Equal(value, criteria.Amount);
    }
}