using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetSiteDBConfigCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Acc_SiteDBConfigSel";

        public override bool IsValid() => CustId > 0;
    }
}