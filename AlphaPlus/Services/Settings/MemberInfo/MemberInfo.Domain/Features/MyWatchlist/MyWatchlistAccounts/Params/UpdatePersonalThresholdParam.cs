namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class UpdatePersonalThresholdParam
    {
        public UpdatePersonalThresholdParam(
            int custId,
            string username,
            decimal? winningThreshold,
            decimal? losingThreshold,
            int userId,
            int subAccId,
            string ip,
            string siteName)
        {
            CustId = custId;
            Username = username;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            SiteName = siteName;
        }

        public int CustId { get; }

        public string Username { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string SiteName { get; }
    }
}
