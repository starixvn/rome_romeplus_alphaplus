namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class DeletePersonalThresholdParam
    {
        public DeletePersonalThresholdParam(
           int custId,
           string username,
           int userId,
           int subAccId,
           string ip,
           string siteName)
        {
            CustId = custId;
            Username = username;
            UserId = userId;
            SubAccId = subAccId;
            IP = ip;
            SiteName = siteName;
        }

        public int CustId { get; }

        public string Username { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string SiteName { get; }
    }
}
