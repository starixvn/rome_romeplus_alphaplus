using System.Data.SqlTypes;

namespace AlphaPlus.BetLists.WinLoss;

using Core.Criteria;
using Core.Helpers;

#pragma warning disable S107 // Methods should not have too many parameters

public class GetWinLossWeekDataCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    string suffix
#pragma warning restore S107 // Methods should not have too many parameters
    ) : SpSuffixBaseCriteria(suffix)
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public string ProductIds { get; } = productIds;

    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public bool IsReward { get; } = false;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_WinlossDetail_WeekDataSel", Suffix);

    public override bool IsValid()
        => CustId > 0 && FromDate <= ToDate && FromDate > SqlDateTime.MinValue.Value && !string.IsNullOrWhiteSpace(Username);
}