using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.IPRange;

internal class GetIPRangeV4Criteria(string ipList) : CriteriaBase
{
    public string IPList { get; } = ipList;

    public override string GetSettingKey()
        => "Age_IPRangesByCountrySelByIPList";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(IPList);
}
