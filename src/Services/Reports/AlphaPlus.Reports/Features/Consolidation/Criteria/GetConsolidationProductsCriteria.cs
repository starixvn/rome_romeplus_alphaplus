using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Consolidation;

internal class GetConsolidationProductsCriteria(int custId, byte settingStatus, string cashServiceSites ) : CriteriaBase
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public string CashServiceSites { get; } = cashServiceSites;

    public override string GetSettingKey() => "Age_Rpt_ConsolidatedGroupProductSel";

    public override bool IsValid() => CustId > 0;
}