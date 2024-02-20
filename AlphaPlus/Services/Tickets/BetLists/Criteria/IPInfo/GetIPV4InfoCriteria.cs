using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Criteria.IPInfo;

public class GetIPV4InfoCriteria(string ipList) : CriteriaBase
{
    public string IPList { get; } = ipList;

    public override string GetSettingKey() => "Age_IPRangesByCountrySelByIPList";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(IPList);
}
