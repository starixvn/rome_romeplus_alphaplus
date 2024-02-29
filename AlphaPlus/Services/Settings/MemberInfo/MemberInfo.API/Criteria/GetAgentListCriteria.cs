namespace AlphaPlus.Settings.MemberInfo.API.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAgentListCriteria : CriteriaBase
    {
        public GetAgentListCriteria(int custId)
        {
            CustId = custId;
        }

        [SpParam(SqlParamName = "MRecommend")]
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelAgentList";

        public override bool IsValid()
            => CustId > 0;
    }
}
