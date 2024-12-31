using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WhiteLabel;

internal class GetSettlementCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string whiteLabels,
    string username) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string WhiteLabelList { get; } = whiteLabels;

    public string Username { get; } = username;

    public override string GetSettingKey() => "Age_Rpt_WhiteLabelSettlement";

    public override bool IsValid()
        => CustId > 0
        && FromDate <= ToDate
        && FromDate > SqlDateTime.MinValue.Value;
}