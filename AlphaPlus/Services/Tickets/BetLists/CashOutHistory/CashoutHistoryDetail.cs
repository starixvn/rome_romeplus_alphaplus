using AlphaPlus.Shared.AppResources.Betlist.Services;

namespace AlphaPlus.BetLists.CashOutHistory;

public class CashoutHistoryDetail(DateTime changeTime, decimal amount, string actionId)
{
    public string ChangeTimeText => changeTime.ToString();

    public decimal CashoutAmount { get; } = amount;

    public string ActionName => actionId switch
    {
        "1" => Resources.CashOut,
        "6" => Resources.CashOutCancelled,
        "8" => Resources.CashOutRecovered,
        _ => actionId
    };
}