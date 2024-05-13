namespace AlphaPlus.Settings.Site.Features.AccountInfo.Models
{
    public class OnlineUser
    {
        public string IP { get; set; }

        public DateTime LoginTime { get; set; }

        public string FormattedLoginTime => LoginTime.ToString("dd MMM hh:mm tt");
    }
}