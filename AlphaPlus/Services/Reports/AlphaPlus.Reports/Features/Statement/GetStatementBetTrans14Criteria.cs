using AlphaPlus.Reports.Features.Shared;

namespace AlphaPlus.Reports.Features.Statement;

public class GetStatementBetTrans14Criteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay) : GetStatementCriteria(custId, fromDate, toDate, ReportDateService.BetTrans14SPSuffix)
{
    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public override string GetSettingKey() => "Statement_Report_GetList_14";
}