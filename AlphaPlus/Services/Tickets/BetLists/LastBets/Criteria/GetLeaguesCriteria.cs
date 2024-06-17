using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

internal class GetLeaguesCriteria(bool isShowAll) : CriteriaBase
{
    public override string GetSettingKey() => "Age_BetList_LastBets_LeagueSel";

    public bool IsShowAll { get; } = isShowAll;

    public override bool IsValid() => true;
}