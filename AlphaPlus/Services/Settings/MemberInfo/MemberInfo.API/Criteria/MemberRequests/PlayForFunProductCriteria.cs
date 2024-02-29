namespace AlphaPlus.Settings.MemberInfo.API.Criteria.MemberRequests
{
    using Fanex.Data.Repository;

    public class PlayForFunProductCriteria : CriteriaBase
    {
        public PlayForFunProductCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_Rpt_PlayForFun_ProductSel";

        public override bool IsValid() => CustId > 0;
    }
}
