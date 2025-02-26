using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds.Criteria
{
    public class SelectMyThresholdCriteria : CriteriaBase
    {
        public SelectMyThresholdCriteria(int custId, short sportType)
        {
            CustId = custId;
            SportType = sportType;
        }

        public int CustId { get; }

        public short SportType { get; }

        public override string GetSettingKey()
            => "Acc_WLMonitored_SMA_Sel";

        public override bool IsValid()
            => CustId > 0;
    }
}
