namespace AlphaPlus.BetLists.WinLoss;

internal class GetBetListWinLossByDateCriteria(
    int custId,
    string productIds,
    string productDetails,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    bool showProgressiveBonus = false,
    int maxTotalRow = 10000,
    bool isExportExcel = false) : GetBetListWinLossByDateBaseCriteria(
        custId,
        productIds,
        productDetails,
        fromDate,
        toDate,
        showProgressiveBonus,
        maxTotalRow,
        isExportExcel)
{
    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public override string GetSettingKey() => "Age_BetList_SelWinLossByDate_Mem";
}