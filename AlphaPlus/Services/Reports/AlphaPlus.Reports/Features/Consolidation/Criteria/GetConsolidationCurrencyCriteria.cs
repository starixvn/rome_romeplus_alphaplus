using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Consolidation;

public class GetConsolidationCurrencyCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_Rpt_ConsolidatedGroupSelCurrency";

    public override bool IsValid() => CustId > 0;
}