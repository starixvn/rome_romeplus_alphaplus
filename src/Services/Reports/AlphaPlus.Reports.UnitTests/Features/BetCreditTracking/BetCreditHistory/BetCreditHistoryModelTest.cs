using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class BetCreditHistoryModelTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var errorCode = 1;
        var errorMessage = "Error message";
        var data = new List<BetCreditHistoryEntity>
        {
            new BetCreditHistoryEntity
            {
                ActionDate = new DateTime(2025, 3, 17),
                WinLostDate = new DateTime(2025, 3, 16),
                RefNo = 123456789,
                ActualStake = 100.50m,
                WinLoss = 50.25m,
                PlayerComm = 10.75m,
                BalanceChanged = 20.00m,
                NewBalance = 120.50m,
                GivenCredit = 30.00m,
                BetCredit = 15.00m,
                Action = "TestAction",
                ActionId = 1,
                IsUserLogActionId = true
            }
        };
        var totalRows = 10;

        // Act
        var model = new BetCreditHistoryModel(errorCode, errorMessage, data, totalRows);

        // Assert
        Assert.Equal(errorCode, model.ErrorCode);
        Assert.Equal(errorMessage, model.ErrorMessage);
        Assert.Equal(data, model.ReportData);
        Assert.Equal(totalRows, model.TotalRows);
    }

    [Fact]
    public void ErrorMessage_ShouldBeSettable()
    {
        // Arrange
        var model = new BetCreditHistoryModel(1, "Initial error message", new List<BetCreditHistoryEntity>(), 10);

        // Act
        model.ErrorMessage = "Updated error message";

        // Assert
        Assert.Equal("Updated error message", model.ErrorMessage);
    }
}
