namespace AlphaPlus.BetLists.WinLoss;

internal class GetBetListWinLossByDateOldCriteria(
    int custId,
    string productIds,
    string productDetails,
    DateTime fromDate,
    DateTime toDate,
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
    public override string GetSettingKey() => "Age_BetList_SelWinLossByDate_Mem_Old";
}
