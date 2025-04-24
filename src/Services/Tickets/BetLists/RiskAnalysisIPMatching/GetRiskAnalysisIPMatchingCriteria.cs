using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RiskAnalysisIPMatching;

internal class GetRiskAnalysisIPMatchingCriteria(
    int custId,
    string ip,
    DateTime date,
    DateTime moveBetsDay) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string IP { get; } = ip;

    public DateTime Date { get; } = date;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_BetList_IPMatching";

    public override bool IsValid()
        => CustId > 0
        && !string.IsNullOrWhiteSpace(IP)
        && Date > SqlDateTime.MinValue.Value;
}
