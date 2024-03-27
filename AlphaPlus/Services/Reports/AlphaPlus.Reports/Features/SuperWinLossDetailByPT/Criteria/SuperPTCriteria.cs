using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public class SuperPTCriteria(string superIds, byte settingStatus) : CriteriaBase
{
    public string Supers { get; } = superIds;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "EAS_GetPTProductBySuper";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Supers);
}