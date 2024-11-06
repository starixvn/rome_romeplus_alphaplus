using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    public class SelectWatchlistAccountsCriteria : CriteriaBase
    {
        public SelectWatchlistAccountsCriteria(
            int agentId,
            DateTime fromDate,
            DateTime toDate,
            string status,
            byte problemAccount,
            string username,
            byte sortType,
            bool isDesc,
            int pageIndex,
            int pageSize,
            int sporttype = 0)
        {
            AgentId = agentId;
            AddedFromDate = fromDate;
            AddedToDate = toDate;
            Status = status;
            ProblemAccount = problemAccount;
            Username = username;
            SortType = sortType;
            IsDesc = isDesc;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public DateTime AddedFromDate { get; }

        public DateTime AddedToDate { get; }

        public string Status { get; }

        public byte ProblemAccount { get; }

        public string Username { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public int Sporttype { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Player_WatchList_Sel";

        public override bool IsValid() => AgentId > 0;
    }
}
