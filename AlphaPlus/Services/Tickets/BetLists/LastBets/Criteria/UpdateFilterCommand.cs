namespace AlphaPlus.BetLists.LastBets;

internal class UpdateFilterCommand(
    int custId,
    int subAccId,
    int filterId,
    string filterName,
    bool isDefault,
    bool isNotify,
    string languageKey,
    string ip) : DeleteFilterCommand(custId, subAccId, filterId, languageKey, ip)
{
    public string FilterName { get; } = filterName;

    public bool IsDefault { get; } = isDefault;

    public bool IsNotify { get; } = isNotify;

    public override string GetSettingKey() => "Age_CustLastBetFilter_Upd";

    public override bool IsValid() => base.IsValid() && !string.IsNullOrWhiteSpace(FilterName);
}