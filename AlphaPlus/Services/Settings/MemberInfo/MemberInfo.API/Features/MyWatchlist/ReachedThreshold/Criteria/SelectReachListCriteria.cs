using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold.Criteria
{
    public class SelectReachListCriteria : CriteriaBase
    {
        public SelectReachListCriteria(
            int agentId,
            string username,
            string status,
            byte sortType,
            bool isDesc,
            int pageSize,
            int pageIndex,
            string xmlReachList,
            int sporttype = 0)
        {
            AgentId = agentId;
            Username = username;
            Status = status;
            SortType = sortType;
            IsDesc = isDesc;
            PageSize = pageSize;
            PageIndex = pageIndex;
            XMLReachList = xmlReachList;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public string Username { get; }

        public string Status { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public int Sporttype { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string XMLReachList { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Player_ReachList_Sel";

        public override bool IsValid() => AgentId > 0;
    }
}
