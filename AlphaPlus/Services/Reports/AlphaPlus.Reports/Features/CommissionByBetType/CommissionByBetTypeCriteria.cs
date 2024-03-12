using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CommissionByBetType;

public class CommissionByBetTypeCriteria(
    UserLevel custLevel,
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_Rpt_CommByBettype_Member"
        : CustLevel.IsMasterLevel()
            ? "Age_Rpt_CommByBettype_Agent"
            : "Age_Rpt_CommByBettype_Master";

    public override bool IsValid()
        => !CustLevel.IsMemberLevel()
        && CustId > 0
        && FromDate <= ToDate;
}
