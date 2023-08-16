using AlphaPlus.User.Models;

namespace AlphaPlus.Web.Models
{
    public class BrowserViewModel
    {
        public bool IsUnsupportedBrowser { get; set; }

        public string DownloadNewBrowserLink { get; set; }

        public UserLevel UserLevel { get; set; }
    }
}