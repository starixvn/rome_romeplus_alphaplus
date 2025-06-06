namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class PotentialWinLimitParam
    {
        public PotentialWinLimitParam(
            int custId,
            string username,
            string status,
            byte problemAccount,
            int pageSize,
            int pageIndex,
            string agIP,
            int betType = 0,
            int sportType = 0)
        {
            CustId = custId;
            BetType = betType;
            SportType = sportType;
            Username = username;
            Status = string.IsNullOrEmpty(status) ? "All" : status;
            ProblemAccount = problemAccount;
            PageSize = pageSize;
            PageIndex = pageIndex;
            AGIP = agIP;
        }

        public int CustId { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public byte ProblemAccount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string AGIP { get; }
    }
}
