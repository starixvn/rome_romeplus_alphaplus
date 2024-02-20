using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Criteria.IPInfo;

public class GetIPV4InfoByTopCountryCriteria(string countryCodes) : CriteriaBase
{
    public string CountryCodes { get; } = countryCodes;

    public override string GetSettingKey() => "Age_IPRangesByCountrySel";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CountryCodes);
}
