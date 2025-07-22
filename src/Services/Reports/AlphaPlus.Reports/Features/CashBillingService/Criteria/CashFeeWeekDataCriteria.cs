using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.CashBillingService.Criteria;

internal class CashFeeWeekDataCriteria(
int custId,
DateTime fromDate,
DateTime toDate,
DateTime balanceUpDay,
DateTime moveBetsDay,
string productIds,
string productDetails,
string username,
bool isReward,
byte levelType) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public bool IsReward { get; } = isReward;

    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";
}
