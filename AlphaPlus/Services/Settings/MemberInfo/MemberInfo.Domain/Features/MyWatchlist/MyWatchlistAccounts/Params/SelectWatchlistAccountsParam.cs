namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class SelectWatchlistAccountsParam
    {
        public SelectWatchlistAccountsParam(
            int agentId,
            DateTime fromDate,
            DateTime toDate,
            string status,
            byte problemAccount,
            string username,
            string siteName,
            byte sortType,
            bool isDesc,
            int pageIndex,
            int pageSize)
        {
            AgentId = agentId;
            FromDate = fromDate;
            ToDate = toDate;
            Status = status;
            ProblemAccount = problemAccount;
            Username = username;
            SortType = sortType;
            IsDesc = isDesc;
            PageIndex = pageIndex;
            PageSize = pageSize;
            SiteName = siteName;
        }

        public int AgentId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Status { get; }

        public byte ProblemAccount { get; }

        public string Username { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SiteName { get; }
    }
}
