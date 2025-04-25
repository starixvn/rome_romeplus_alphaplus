using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class BetCreditHistoryCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var actionDate = new DateTime(2025, 3, 17);
        var winLostDate = new DateTime(2025, 3, 16);
        long? refNo = 123456789;
        decimal? actualStake = 100.50m;
        decimal? winLoss = 50.25m;
        decimal? playerComm = 10.75m;
        decimal? balanceChanged = 20.00m;
        decimal? newBalance = 120.50m;
        decimal givenCredit = 30.00m;
        decimal? betCredit = 15.00m;
        string action = "TestAction";
        int actionId = 1;
        bool isUserLogActionId = true;

        // Act
        var entity = new BetCreditHistoryEntity
        {
            ActionDate = actionDate,
            WinLostDate = winLostDate,
            RefNo = refNo,
            ActualStake = actualStake,
            WinLoss = winLoss,
            PlayerComm = playerComm,
            BalanceChanged = balanceChanged,
            NewBalance = newBalance,
            GivenCredit = givenCredit,
            BetCredit = betCredit,
            Action = action,
            ActionId = actionId,
            IsUserLogActionId = isUserLogActionId
        };

        // Assert
        Assert.Equal(actionDate, entity.ActionDate);
        Assert.Equal(winLostDate, entity.WinLostDate);
        Assert.Equal(refNo, entity.RefNo);
        Assert.Equal(actualStake, entity.ActualStake);
        Assert.Equal(winLoss, entity.WinLoss);
        Assert.Equal(playerComm, entity.PlayerComm);
        Assert.Equal(balanceChanged, entity.BalanceChanged);
        Assert.Equal(newBalance, entity.NewBalance);
        Assert.Equal(givenCredit, entity.GivenCredit);
        Assert.Equal(betCredit, entity.BetCredit);
        Assert.Equal(action, entity.Action);
        Assert.Equal(actionId, entity.ActionId);
        Assert.Equal(isUserLogActionId, entity.IsUserLogActionId);
    }

    [Fact]
    public void RefNoString_ShouldReturnCorrectString()
    {
        // Arrange
        var entityWithRefNo = new BetCreditHistoryEntity { RefNo = 123456789 };
        var entityWithoutRefNo = new BetCreditHistoryEntity { RefNo = null };

        // Act & Assert
        Assert.Equal("123456789", entityWithRefNo.RefNoString);
        Assert.Equal(string.Empty, entityWithoutRefNo.RefNoString);
    }

    [Fact]
    public void ActionDateString_ShouldReturnFormattedDate()
    {
        // Arrange
        var actionDate = new DateTime(2025, 3, 17, 14, 30, 0);
        var entity = new BetCreditHistoryEntity { ActionDate = actionDate };

        // Act
        var actionDateString = entity.ActionDateString;

        // Assert
        Assert.Equal("03/17/2025 14:30:00", actionDateString);
    }

    [Fact]
    public void WinLostDateString_ShouldReturnFormattedDate()
    {
        // Arrange
        var winLostDate = new DateTime(2025, 3, 16);
        var entityWithDate = new BetCreditHistoryEntity { WinLostDate = winLostDate };
        var entityWithoutDate = new BetCreditHistoryEntity { WinLostDate = null };

        // Act & Assert
        Assert.Equal("03/16/2025", entityWithDate.WinLostDateString);
        Assert.Equal(string.Empty, entityWithoutDate.WinLostDateString);
    }
}
