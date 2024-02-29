namespace AlphaPlus.Settings.MemberInfo.API.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetMasterListCriteria : CriteriaBase
    {
        public GetMasterListCriteria(int custId)
        {
            CustId = custId;
        }

        [SpParam(SqlParamName = "SRecommend")]
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelMAgentList";

        public override bool IsValid()
            => CustId > 0;
    }
}
