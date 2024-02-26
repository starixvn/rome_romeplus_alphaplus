using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Shared.Criteria;

public class GetProductsCriteria(string site, int currencyId, byte settingStatus) : CriteriaBase
{
    public string Site { get; } = site;

    public int Currency { get; } = currencyId;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "Age_GetOfferedProductBySiteCurrency";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Site) && Currency > 0;
}