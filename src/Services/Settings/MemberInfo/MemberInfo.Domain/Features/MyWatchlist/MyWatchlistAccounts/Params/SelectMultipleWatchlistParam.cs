namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class SelectMultipleWatchlistParam
    {
        public SelectMultipleWatchlistParam(string custList, string siteName)
        {
            CustList = custList;
            SiteName = siteName;
        }

        public string CustList { get; }

        public string SiteName { get; }
    }
}
