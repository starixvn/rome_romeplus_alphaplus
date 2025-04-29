using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.MemberSite
{
    public class GetMemberSiteInfoCriteria(int custId) : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_GeneralSetting";

        public int CustId { get; set; } = custId;

        public override bool IsValid() => CustId > 0;
    }
}