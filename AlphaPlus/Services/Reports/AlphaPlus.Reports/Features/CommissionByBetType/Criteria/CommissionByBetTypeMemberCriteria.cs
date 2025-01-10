using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CommissionByBetType;

internal class CommissionByBetTypeMemberCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_Rpt_CommByBettype_Member";

    public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
}
