namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Params
{
    using System;

    public class SelectReachListParam
    {
        public SelectReachListParam(
           int agentId,
           string username,
           DateTime fromDate,
           DateTime toDate,
           string status,
           byte limitType,
           byte sortType,
           bool isDesc,
           int pageSize,
           int pageIndex,
           DateTime limitedDateToShowInfo,
           string siteName)
        {
            AgentId = agentId;
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            Status = status;
            LimitType = limitType;
            SortType = sortType;
            IsDesc = isDesc;
            PageSize = pageSize;
            PageIndex = pageIndex;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            SiteName = siteName;
        }

        public int AgentId { get; }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Status { get; }

        public byte LimitType { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public string SiteName { get; }
    }
}
