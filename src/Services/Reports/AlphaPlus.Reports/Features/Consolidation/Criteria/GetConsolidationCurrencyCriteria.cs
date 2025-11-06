namespace AlphaPlus.Reports.Features.Consolidation;

using Core.Criteria;
using Core.Helpers;

internal class GetConsolidationCurrencyCriteria(int custId, string suffix) : SpSuffixBaseCriteria(suffix)
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => SpNameHelpers.Suffix("Age_Rpt_ConsolidatedGroupSelCurrency", Suffix);

    public override bool IsValid() => CustId > 0;
}