namespace AlphaPlus.Reports.Features.WhiteLabel;

internal class GetDailyWinLossCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string whiteLabels,
    string username) : GetSettlementCriteria(custId, fromDate, toDate, whiteLabels, username)
{
    public override string GetSettingKey() => "Age_Rpt_WhiteLabelWinLoss";
}