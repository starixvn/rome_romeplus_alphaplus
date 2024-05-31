using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.BetsForecast;

public class GetBetListRunningByBetTypeAndMatchCriteria(
    int matchId,
    int custId,
    int betType,
    int liveIndicator,
    string betTeam,
    UserLevel custLevel) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public int LiveIndicator { get; } = liveIndicator;

    public string BetTeam { get; } = betTeam;

    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public override string GetSettingKey()
       => CustLevel.IsAgentLevel()
        ? "Age_BetList_SelRunningByBettypeMatch_Ag"
        : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelRunningByBettypeMatch_MAg"
            : "Age_BetList_SelRunningByBettypeMatch_SAg";

    public override bool IsValid() => CustId > 0 && BetType > 0;
}
