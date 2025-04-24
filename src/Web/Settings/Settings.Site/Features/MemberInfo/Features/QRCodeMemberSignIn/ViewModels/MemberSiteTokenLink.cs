namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.ViewModels
{
    public class MemberSiteTokenLink
    {
        public MemberSiteTokenLink(
            string qrCode,
            string memberSiteLink)
        {
            QRCode = qrCode;
            MemberSiteLink = memberSiteLink;
        }

        public string QRCode { get; }

        public string MemberSiteLink { get; }
    }
}