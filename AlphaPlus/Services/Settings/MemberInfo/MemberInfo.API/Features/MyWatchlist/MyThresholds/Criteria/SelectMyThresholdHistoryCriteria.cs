namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class SelectMyThresholdHistoryCriteria : CriteriaBase
    {
        public SelectMyThresholdHistoryCriteria(
            int custId,
            short sportType,
            DateTime fromDate,
            DateTime toDate,
            byte sortType,
            bool isDesc)
        {
            CustId = custId;
            SportType = sportType;
            FromDate = fromDate;
            ToDate = toDate;
            SortType = sortType;
            IsDesc = isDesc;
        }

        public int CustId { get; }

        public short SportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte SortType { get; }

        public bool IsDesc { get; }

        public override string GetSettingKey()
            => "Acc_WLMonitored_SMA_ReachDetail_Sel";

        public override bool IsValid()
            => CustId > 0 && FromDate <= ToDate;
    }
}
