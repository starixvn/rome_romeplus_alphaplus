using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.BetList;

internal class GetOddsOfMixParlayCriteria(string transIdList) : CriteriaBase
{
    public string TransIdList { get; } = transIdList;

    public override string GetSettingKey()
        => "Age_BetList_MixParlay_OddsSel";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(TransIdList);
}
