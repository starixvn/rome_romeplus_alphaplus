namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels
{
    public class QRCodeLoginConfigs
    {
        public QRCodeLoginConfigs(
            string[] domains,
            int siteId,
            int expireSeconds)
        {
            Domains = domains;
            SiteId = siteId;
            ExpireSeconds = expireSeconds;
        }
        public string[] Domains { get; set; }
        public int SiteId { get; set; }
        public int ExpireSeconds { get; set; }
    }
}