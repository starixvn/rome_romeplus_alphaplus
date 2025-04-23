using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Consolidation;

internal class GetSMAPositionTakingCriteria(string custIds, byte settingStatus) : CriteriaBase
{
    public string CustIds { get; } = custIds;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "EAS_GetPTProductBySMA";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CustIds);
}