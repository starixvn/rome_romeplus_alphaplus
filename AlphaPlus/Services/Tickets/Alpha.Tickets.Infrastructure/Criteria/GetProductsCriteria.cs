using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria;

internal class GetProductsCriteria(string site, int currency, byte settingStatus) : CriteriaBase
{
    public string Site { get; } = site;

    public int Currency { get; } = currency;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey()
        => "Age_GetOfferedProductBySiteCurrency";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(Site) && Currency > 0;
}