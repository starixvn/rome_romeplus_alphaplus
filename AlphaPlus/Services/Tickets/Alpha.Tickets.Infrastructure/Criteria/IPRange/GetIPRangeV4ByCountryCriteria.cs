using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.IPRange;

internal class GetIPRangeV4ByCountryCriteria(string countryCodes) : CriteriaBase
{
    public string CountryCodes { get; } = countryCodes;

    public override string GetSettingKey()
        => "Age_IPRangesByCountrySel";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CountryCodes);
}

