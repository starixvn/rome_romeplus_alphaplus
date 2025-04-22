using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class GetLastBetsNewCriteria(int custId,
    long transId,
    string memberList,
    string sportTypeList,
    string leagueList,
    string betTypeList,
    byte liveIndicator,
    decimal minStake,
    int limitRowCount,
    bool isSuperLevel,
    bool isMasterLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public string MemberList { get; } = memberList;

    public string BetTypeList { get; } = betTypeList;

    public string SportTypeList { get; } = sportTypeList ?? string.Empty;

    public string LeagueList { get; } = leagueList ?? string.Empty;

    public byte LiveIndicator { get; } = liveIndicator;

    public decimal MinStake { get; } = minStake;

    public int LimitRowCount { get; } = limitRowCount;

    public override string GetSettingKey() => isSuperLevel
        ? "Age_BetList_LastBets_SAg" 
        : isMasterLevel
            ? "Age_BetList_LastBets_MAg" 
            : "Age_BetList_LastBets_Ag";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(BetTypeList) && MinStake >= 0 && LimitRowCount > 0;
}