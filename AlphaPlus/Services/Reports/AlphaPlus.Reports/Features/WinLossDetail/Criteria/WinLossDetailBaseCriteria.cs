using AlphaPlus.Reports.Features.Shared.Criteria;

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
    bool isReward) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public bool IsReward { get; } = isReward;
}