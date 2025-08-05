using AlphaPlus.CashService.API.Features.Notifications.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Notifications.Criteria;

[Trait("Category", "Notifications")]
public class GetTransactionPendingCountCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalled()
    {
        // Arrange
        var custId = 123;
        var typeId = (byte)2;

        // Act
        var criteria = new GetTransactionPendingCountCriteria(custId, typeId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(typeId, criteria.TypeId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetTransactionPendingCountCriteria(1, 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashTransaction_PendingCount", result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, byte typeId, bool expected)
    {
        // Arrange
        var criteria = new GetTransactionPendingCountCriteria(custId, typeId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
