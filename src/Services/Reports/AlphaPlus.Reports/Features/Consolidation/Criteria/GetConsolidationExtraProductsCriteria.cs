namespace AlphaPlus.Reports.Features.Consolidation;

using Core.Criteria;
using Core.Helpers;

internal class GetConsolidationExtraProductsCriteria(int custId, byte settingStatus, string cashServiceSites, string spSuffix) : SpSuffixBaseCriteria(spSuffix)
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public string CashServiceSites { get; } = cashServiceSites;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_ConsolidatedGroupProductSel_Extra", Suffix);

    public override bool IsValid() => CustId > 0;
}