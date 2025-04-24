using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.MemberSite
{
    public class GetMemberSiteInfoCriteria : CriteriaBase
    {
        public GetMemberSiteInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public override string GetSettingKey() => "Age_CashService_GeneralSetting";

        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; set; }

        public override bool IsValid() => true;
    }
}