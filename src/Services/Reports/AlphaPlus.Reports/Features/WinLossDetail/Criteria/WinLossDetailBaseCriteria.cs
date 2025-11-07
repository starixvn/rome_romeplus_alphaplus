using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.WinLossDetail;

internal abstract class WinLossDetailBaseCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward,
    string suffix) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public bool IsReward { get; } = isReward;

    [SpParam(Ignored = true)]
    public string Suffix { get; set; } = suffix;
}