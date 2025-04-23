using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RunningBetListByBetTypeAndMatch;

internal class GetBetListRunningByBetTypeAndMatchCriteria(
    int matchId,
    int custId,
    int betType,
    int liveIndicator,
    string betTeam,
    byte custLevelId) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public int LiveIndicator { get; } = liveIndicator;

    public string BetTeam { get; } = betTeam;

    public override string GetSettingKey()
       => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_BetList_SelRunningByBettypeMatch_Ag"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_BetList_SelRunningByBettypeMatch_MAg"
            : "Age_BetList_SelRunningByBettypeMatch_SAg";

    public override bool IsValid() => CustId > 0 && BetType > 0;
}
