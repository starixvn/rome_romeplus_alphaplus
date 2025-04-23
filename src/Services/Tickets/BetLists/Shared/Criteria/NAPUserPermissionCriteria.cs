using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class NAPUserPermissionCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_Con_FileNameSelByCustID";

    public override bool IsValid() => CustId > 0;
}