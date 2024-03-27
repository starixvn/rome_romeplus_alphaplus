using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public class WinLossDetailCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    UserLevel custLevel,
    string productDetails,
    string username) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public int PageIndex { get; } = 1;

    public int PageSize { get; } = 10000;

    public int TotalRecords { get; set; }

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_Rpt_WinLossDetail_Member"
        : CustLevel.IsMasterLevel()
            ? "Age_Rpt_WinLossDetail_Agent"
            : "Age_Rpt_WinLossDetail_Master";
}