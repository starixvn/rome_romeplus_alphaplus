namespace AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.ViewModels
{
    public class QRCodeLoginConfigs
    {
        public QRCodeLoginConfigs(
            string api,
            string[] domains,
            int siteId,
            int expireSeconds)
        {
            API = api;
            Domains = domains;
            SiteId = siteId;
            ExpireSeconds = expireSeconds;
        }

        public string API { get; set; }
        public string[] Domains { get; set; }
        public int SiteId { get; set; }
        public int ExpireSeconds { get; set; }
    }
}