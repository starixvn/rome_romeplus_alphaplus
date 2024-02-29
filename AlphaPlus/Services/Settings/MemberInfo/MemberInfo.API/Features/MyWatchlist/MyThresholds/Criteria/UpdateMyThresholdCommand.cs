namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateMyThresholdCommand : NonQueryCommand
    {
        public UpdateMyThresholdCommand(
            int custId,
            short sportType,
            decimal? winningThreshold,
            decimal? losingThreshold,
            int userId,
            int subAccId,
            string ip)
        {
            CustId = custId;
            SportType = sportType;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
        }

        public int CustId { get; }

        public short SportType { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string CustList { get; set; }

        public override string GetSettingKey()
            => "Acc_WLMonitored_SMA_Adjust";

        public override bool IsValid()
            => CustId > 0;
    }
}
