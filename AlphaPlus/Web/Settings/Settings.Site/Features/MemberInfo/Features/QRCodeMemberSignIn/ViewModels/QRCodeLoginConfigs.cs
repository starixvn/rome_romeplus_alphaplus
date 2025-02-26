namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels
{
    public class QRCodeLoginConfigs
    {
        public string API { get; set; }
        public string[] Domains { get; set; }
        public int SiteId { get; set; }
        public int ExpireSeconds { get; set; }
    }
}