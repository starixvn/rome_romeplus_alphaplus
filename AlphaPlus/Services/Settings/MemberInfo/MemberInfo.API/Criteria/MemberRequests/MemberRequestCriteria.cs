using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.MemberRequests
{
    public class MemberRequestCriteria : CriteriaBase
    {
        public MemberRequestCriteria(
            int custId,
            int productId)
        {
            CustId = custId;
            ProductId = productId;
        }

        public int CustId { get; }

        public int ProductId { get; }

        public override string GetSettingKey() => "Age_PlayForFun_MemberRequest_Detail";

        public override bool IsValid() => CustId > 0 && ProductId > 0;
    }
}