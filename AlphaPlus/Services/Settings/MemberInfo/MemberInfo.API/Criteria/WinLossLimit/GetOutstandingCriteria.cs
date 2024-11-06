using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class GetOutstandingCriteria : CriteriaBase
    {
        public GetOutstandingCriteria(
            int custId,
            int productId,
            int betType,
            int sportType,
            string username,
            int masterId,
            int agentId,
            int? playerId,
            int minCustId,
            int maxCustId)
        {
            PlayerId = playerId;

            CustId = custId;
            ProductId = productId;
            BetType = betType;
            SportType = sportType;
            Username = username;
            MasterId = masterId;
            AgentId = agentId;
            MinCustId = minCustId;
            MaxCustId = maxCustId;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public int MinCustId { get; set; }

        public int MaxCustId { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public string Username { get; set; }

        [SpParam(SqlParamName = "Mrecommend")]
        public int MasterId { get; set; }

        [SpParam(SqlParamName = "Recommend")]
        public int AgentId { get; set; }

        [SpParam(SqlParamName = "PlayerId_IBC")]
        public int? PlayerId { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelWinlimit_Outstanding";

        public override bool IsValid()
            => CustId > 0;
    }
}
