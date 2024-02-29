namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class WinLossLimitParam
    {
        private const int SabaclubProductId = 42;

        public WinLossLimitParam(
            int productId,
            int custId,
            int betType,
            int sportType,
            string username,
            string playerId,
            int masterId,
            int agentId,
            int pageSize,
            int pageIndex,
            string agIP)
        {
            ProductId = productId;
            CustId = custId;
            BetType = betType;
            SportType = sportType;
            Username = username;
            PlayerId = playerId;
            MasterId = masterId;
            AgentId = agentId;
            PageSize = pageSize;
            PageIndex = pageIndex;
            AGIP = agIP;
        }

        public int ProductId { get; set; }

        public bool IsSabaclub => ProductId == SabaclubProductId;

        public int CustId { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public string Username { get; set; }

        public string PlayerId { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string AGIP { get; }
    }
}
