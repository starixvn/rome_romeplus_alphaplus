namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyThresholds.Params
{
    public class UpdateMyThresholdParam
    {
        public UpdateMyThresholdParam(
            int custId,
            decimal? winningThreshold,
            decimal? losingThreshold,
            int userId,
            int subAccId,
            string ip,
            string siteName,
            short sportType = 0)
        {
            CustId = custId;
            SportType = sportType;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            SiteName = siteName;
        }
        public string SiteName { get; }

        public int CustId { get; }

        public short SportType { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }
    }
}
