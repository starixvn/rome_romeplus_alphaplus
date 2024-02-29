namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyThresholds.Params
{
    using System;

    public class SelectMyThresholdHistoryParam
    {
        public SelectMyThresholdHistoryParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            byte sortType,
            bool isDesc,
            DateTime limitedDateToShowInfo,
            string siteName,
            short sportType = 0)
        {
            CustId = custId;
            SportType = sportType;
            FromDate = fromDate;
            ToDate = toDate;
            SortType = sortType;
            IsDesc = isDesc;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            SiteName = siteName;
        }

        public string SiteName { get; }

        public int CustId { get; }

        public short SportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}
