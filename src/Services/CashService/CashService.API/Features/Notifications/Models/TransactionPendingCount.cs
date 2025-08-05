namespace AlphaPlus.CashService.API.Features.Notifications.Models;

public class TransactionPendingCount
{
    public int PendingDeposit { get; init; }
    public int PendingWithdrawal { get; init; }
    public int PendingPromotion { get; init; }
}