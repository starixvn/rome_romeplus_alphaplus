using AgencyCash.API.Payments.Criteria.Withdrawal;

namespace AgencyCash.API.Tests.Payments.Criteria.Withdrawal;

[Trait("Category", "Withdrawal")]
public class GetDailyWithdrawalCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        const int custId = 123;
        const short siteDBGroupId = 456;

        // Act
        var criteria = new GetDailyWithdrawalCriteria(custId, siteDBGroupId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(siteDBGroupId, criteria.SiteDBGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SPName()
    {
        // Arrange
        const int custId = 123;
        const short siteDBGroupId = 456;
        var criteria = new GetDailyWithdrawalCriteria(custId, siteDBGroupId);
        var expectedSpName = "SiteDB456_Age_CashMember_DailyWithdrawalCount_Sel";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSpName, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, bool expectedIsValid)
    {
        // Arrange
        const short siteDBGroupId = 456;
        var criteria = new GetDailyWithdrawalCriteria(custId, siteDBGroupId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedIsValid, result);
    }

    [Fact]
    public void WithdrawalCnt_Property_Should_Set_And_Get_Value()
    {
        // Arrange
        const int custId = 123;
        const short siteDBGroupId = 456;
        var criteria = new GetDailyWithdrawalCriteria(custId, siteDBGroupId);
        const int expectedCount = 5;

        // Act
        criteria.WithdrawalCnt = expectedCount;

        // Assert
        Assert.Equal(expectedCount, criteria.WithdrawalCnt);
    }
}