using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.SignIn;

public class GetSiteDBConfigCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_SiteDBConfigSel";

    public override bool IsValid() => CustId > 0;
}
