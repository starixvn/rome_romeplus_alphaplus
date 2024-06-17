using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLoss;

internal class GetWinLossWeekDataCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public string ProductIds { get; } = productIds;

    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";

    public override bool IsValid()
        => CustId > 0 && FromDate <= ToDate && FromDate > SqlDateTime.MinValue.Value && !string.IsNullOrWhiteSpace(Username);
}
