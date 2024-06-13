using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetProductsByGroupHeadlineCriteria(int custId, byte settingStatus) : CriteriaBase
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "Age_Rpt_ConsolidatedGroupProductSel";

    public override bool IsValid() => CustId > 0;
}
