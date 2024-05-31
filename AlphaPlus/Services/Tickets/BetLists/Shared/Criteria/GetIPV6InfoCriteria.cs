using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetIPV6InfoCriteria(string ipList) : CriteriaBase
{
    public string IPList { get; } = ipList;

    public override string GetSettingKey() => "Age_IPRangesByCountrySelByIPV6List";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(IPList);
}
