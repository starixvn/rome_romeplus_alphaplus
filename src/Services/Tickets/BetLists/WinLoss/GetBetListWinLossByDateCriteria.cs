namespace AlphaPlus.BetLists.WinLoss;

using Core.Helpers;
using Fanex.Data;

internal class GetBetListWinLossByDateCriteria(
    int custId,
    string productIds,
    string productDetails,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    bool showProgressiveBonus = false,
    int maxTotalRow = 10000,
    bool isExportExcel = false,
    string suffix = "") : GetBetListWinLossByDateBaseCriteria(
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

    [SpParam(Ignored = true)]
    public string Suffix { get; } = suffix;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_BetList_SelWinLossByDate_Mem", Suffix);
}