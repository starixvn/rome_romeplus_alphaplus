using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold.Criteria
{
    public class SelectReachDetailCriteria : CriteriaBase
    {
        public SelectReachDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            byte limitType,
            byte sortType,
            bool isDesc,
            int sporttype = 0)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            LimitType = limitType;
            SortType = sortType;
            IsDesc = isDesc;
            Sporttype = sporttype;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LimitType { get; }

        public int Sporttype { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public override string GetSettingKey() => "Acc_WLMonitored_Player_ReachDetail_Sel";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}
