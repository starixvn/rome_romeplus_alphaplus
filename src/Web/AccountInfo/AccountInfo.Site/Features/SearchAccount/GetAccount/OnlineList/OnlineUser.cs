namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList
{
    public class OnlineUser
    {
        public string IP { get; set; }

        public DateTime LoginTime { get; set; }

        public string FormattedLoginTime => LoginTime.ToString("dd MMM hh:mm tt");
    }
}