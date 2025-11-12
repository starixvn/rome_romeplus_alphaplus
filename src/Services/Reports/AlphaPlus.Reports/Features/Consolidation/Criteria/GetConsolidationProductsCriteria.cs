using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Consolidation;

internal class GetConsolidationProductsCriteria(int custId, byte settingStatus) : CriteriaBase
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "Age_Rpt_ConsolidatedGroupProductSel";

    public override bool IsValid() => CustId > 0;
}