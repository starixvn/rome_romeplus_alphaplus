namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBalanceCriteria : CriteriaBase
    {
        public GetBalanceCriteria(
            int custId,
            int betType,
            string username,
            int masterId,
            int agentId,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            BetType = betType;
            Username = username;
            MasterId = masterId;
            AgentId = agentId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; set; }

        public int BetType { get; set; }

        public string Username { get; set; }

        [SpParam(SqlParamName = "Mrecommend")]
        public int MasterId { get; set; }

        [SpParam(SqlParamName = "Recommend")]
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Age_CustomerSelWinlimitInfo";

        public override bool IsValid()
            => CustId > 0;
    }
}
