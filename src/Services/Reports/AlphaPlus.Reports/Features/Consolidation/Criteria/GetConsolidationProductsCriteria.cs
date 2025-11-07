namespace AlphaPlus.Reports.Features.Consolidation;

using Core.Criteria;
using Core.Helpers;

public class GetConsolidationProductsCriteria(int custId, byte settingStatus, string suffix) : SpSuffixBaseCriteria(suffix)
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_ConsolidatedGroupProductSel", Suffix);

    public override bool IsValid() => CustId > 0;
}