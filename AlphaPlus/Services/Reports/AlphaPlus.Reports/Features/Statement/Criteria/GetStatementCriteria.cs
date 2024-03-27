using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Statement;

public class GetStatementCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string storeMappingSuffix) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    [SpParam(Ignored = true)]
    public string StoreMappingSuffix { get; } = storeMappingSuffix;

    public override string GetSettingKey() => $"Statement_Report_GetList{StoreMappingSuffix}";

    public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
}