using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.IPRange;

internal class GetIPRangeV6Criteria(string ipList) : CriteriaBase
{
    public string IPList { get; } = ipList;

    public override string GetSettingKey()
        => "Age_IPRangesByCountrySelByIPV6List";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(IPList);
}
