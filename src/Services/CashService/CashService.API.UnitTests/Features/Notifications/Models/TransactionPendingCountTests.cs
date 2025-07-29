using AlphaPlus.CashService.API.Features.Notifications.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Notifications.Models;

[Trait("Category", "Models")]
public class TransactionPendingCountTests
{
    [Fact]
    public void TransactionPendingCount_PropertiesInitialization_ShouldWorkAsExpected()
    {
        // Arrange
        var expected = new { PendingDeposit = 5, PendingWithdrawal = 3, PendingPromotion = 2 };

        // Act
        var pendingCount = new TransactionPendingCount
        {
            PendingDeposit = expected.PendingDeposit,
            PendingWithdrawal = expected.PendingWithdrawal,
            PendingPromotion = expected.PendingPromotion
        };

        // Assert
        Assert.Equal(expected.PendingDeposit, pendingCount.PendingDeposit);
        Assert.Equal(expected.PendingWithdrawal, pendingCount.PendingWithdrawal);
        Assert.Equal(expected.PendingPromotion, pendingCount.PendingPromotion);
    }

    [Fact]
    public void TransactionPendingCount_DefaultValues_ShouldBeZero()
    {
        // Act
        var pendingCount = new TransactionPendingCount();

        // Assert
        Assert.Equal(0, pendingCount.PendingDeposit);
        Assert.Equal(0, pendingCount.PendingWithdrawal);
        Assert.Equal(0, pendingCount.PendingPromotion);
    }

    [Theory]
    [InlineData(10, 5, 3)]
    [InlineData(0, 0, 0)]
    [InlineData(100, 200, 300)]
    public void TransactionPendingCount_DifferentValues_ShouldInitializeCorrectly(
        int pendingDeposit, int pendingWithdrawal, int pendingPromotion)
    {
        // Act
        var pendingCount = new TransactionPendingCount
        {
            PendingDeposit = pendingDeposit,
            PendingWithdrawal = pendingWithdrawal,
            PendingPromotion = pendingPromotion
        };

        // Assert
        Assert.Equal(pendingDeposit, pendingCount.PendingDeposit);
        Assert.Equal(pendingWithdrawal, pendingCount.PendingWithdrawal);
        Assert.Equal(pendingPromotion, pendingCount.PendingPromotion);
    }
}