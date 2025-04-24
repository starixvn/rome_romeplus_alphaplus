using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetIPV4InfoByTopCountryCriteria(string countryCodes) : CriteriaBase
{
    public string CountryCodes { get; } = countryCodes;

    public override string GetSettingKey() => "Age_IPRangesByCountrySel";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CountryCodes);
}
