using AgencyCash.API.Payments.Criteria.Withdrawal;

namespace AgencyCash.API.Tests.Payments.Criteria.Withdrawal;

[Trait("Category", "Withdrawal")]
public class WithdrawalLimitAmountCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var uplineId = 456;
        var isMemberSite = true;
        short custDBGroupId = 7;

        // Act
        var criteria = new WithdrawalLimitAmountCriteria(
            custId,
            uplineId,
            isMemberSite,
            custDBGroupId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(uplineId, criteria.UplineId);
        Assert.Equal(isMemberSite, criteria.IsMemberSite);
        Assert.Equal(custDBGroupId, criteria.CustDBGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_StoredProcedureName()
    {
        // Arrange
        short custDBGroupId = 5;
        var criteria = new WithdrawalLimitAmountCriteria(1, 1, false, custDBGroupId);
        var expectedSpName = $"CustDB{custDBGroupId}_Age_CashWithdrawal_LimitAmtSel";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSpName, result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(100, 200, true)]
    public void IsValid_Should_Return_True_When_CustId_And_UplineId_GreaterThan_Zero(int custId, int uplineId, bool expectedResult)
    {
        // Arrange
        var criteria = new WithdrawalLimitAmountCriteria(custId, uplineId, false, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    [InlineData(-1, 5, false)]
    [InlineData(5, -1, false)]
    public void IsValid_Should_Return_False_When_CustId_Or_UplineId_NotGreaterThan_Zero(int custId, int uplineId, bool expectedResult)
    {
        // Arrange
        var criteria = new WithdrawalLimitAmountCriteria(custId, uplineId, false, 1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}