using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CommissionByBetType;

internal class CommissionByBetTypeCriteria(
    byte custLevelId,
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_Rpt_CommByBettype_Member"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_Rpt_CommByBettype_Agent"
            : "Age_Rpt_CommByBettype_Master";

    public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
}
