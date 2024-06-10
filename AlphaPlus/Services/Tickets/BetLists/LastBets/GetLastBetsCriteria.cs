using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class GetLastBetsCriteria(
    int betType,
    int custId,
    long transId,
    int pageSize) : CriteriaBase
{
    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public int LimitRowCount { get; } = pageSize;

    public override string GetSettingKey()
        => betType switch
        {
            BetTypeConstants.AllBetTypes => "Ost_LastBets_All",
            BetTypeConstants.Handicap => "Ost_LastBets_HDP",
            BetTypeConstants.OverUnder => "Ost_LastBets_OU",
            _ => "Ost_LastBets_Other"
        };

    public override bool IsValid() => CustId > 0 && LimitRowCount > 0;
}
