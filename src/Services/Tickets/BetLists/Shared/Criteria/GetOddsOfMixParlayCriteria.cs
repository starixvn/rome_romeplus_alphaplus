using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetOddsOfMixParlayCriteria(string transIds) : CriteriaBase
{
    public string TransIdList { get; } = transIds;

    public override string GetSettingKey() => "Age_BetList_MixParlay_OddsSel";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(TransIdList);
}
