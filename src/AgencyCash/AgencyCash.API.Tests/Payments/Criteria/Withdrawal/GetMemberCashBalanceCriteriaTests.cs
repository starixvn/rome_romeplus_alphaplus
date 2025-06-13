using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria.Withdrawal;

[Trait("Category", "Withdrawal")]
public class GetMemberCashBalanceCriteriaTest
{
    private const int ValidCustId = 123;
    private const short ValidSiteDBGroupId = 1;

    [Fact]
    public void Constructor_Should_Set_Properties()
    {
        // Act
        var criteria = new GetMemberCashBalanceCriteria(ValidCustId, ValidSiteDBGroupId);

        // Assert
        Assert.Equal(ValidCustId, criteria.CustId);
        Assert.Equal(ValidSiteDBGroupId, criteria.SiteDBGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_SPName()
    {
        // Arrange
        var criteria = new GetMemberCashBalanceCriteria(ValidCustId, ValidSiteDBGroupId);
        var expectedSpName = SPNameHelper.SiteDBPrefix("Age_CashService_CashBalance_Sel", ValidSiteDBGroupId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSpName, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_CustId_Is_Greater_Than_Zero()
    {
        // Arrange
        var criteria = new GetMemberCashBalanceCriteria(ValidCustId, ValidSiteDBGroupId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Zero()
    {
        // Arrange
        var criteria = new GetMemberCashBalanceCriteria(0, ValidSiteDBGroupId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Negative()
    {
        // Arrange
        var criteria = new GetMemberCashBalanceCriteria(-1, ValidSiteDBGroupId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}