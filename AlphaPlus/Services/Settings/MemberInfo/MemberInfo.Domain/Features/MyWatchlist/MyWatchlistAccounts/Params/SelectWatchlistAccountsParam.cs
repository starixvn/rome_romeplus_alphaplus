namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class SelectWatchlistAccountsParam
    {
        public SelectWatchlistAccountsParam(
            int agentId, 
            string username,
            string siteName,
            byte sortType,
            bool isDesc,
            int pageIndex,
            int pageSize)
        {
            AgentId = agentId;
            Username = username;
            SortType = sortType;
            IsDesc = isDesc;
            PageIndex = pageIndex;
            PageSize = pageSize;
            SiteName = siteName;
        }

        public string SiteName { get; }

        public int AgentId { get; }

        public string Username { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}
