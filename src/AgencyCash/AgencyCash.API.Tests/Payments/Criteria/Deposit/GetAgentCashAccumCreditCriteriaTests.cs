using AgencyCash.API.Payments.Criteria.Deposit;

namespace AgencyCash.API.Tests.Payments.Criteria.Deposit;

[Trait("Category", "Deposit")]
public class GetAgentCashAccumCreditCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int custId = 123;
        short siteDBGroupId = 5;

        // Act
        var criteria = new GetAgentCashAccumCreditCriteria(custId, siteDBGroupId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(0m, criteria.AccumCreditReturn);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SPName()
    {
        // Arrange
        int custId = 123;
        short siteDBGroupId = 5;
        var criteria = new GetAgentCashAccumCreditCriteria(custId, siteDBGroupId);
        string expectedSPName = "SiteDB5_Age_CashAccumCredit_AgSel";

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSPName, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_ExpectedResult(int custId, bool expected)
    {
        // Arrange
        short siteDBGroupId = 5;
        var criteria = new GetAgentCashAccumCreditCriteria(custId, siteDBGroupId);

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}