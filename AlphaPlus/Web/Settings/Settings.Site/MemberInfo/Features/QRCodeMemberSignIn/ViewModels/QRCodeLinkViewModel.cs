namespace AlphaPlus.Settings.Site.Features.QRCodeMemberSignIn.ViewModels
{
    public class QRCodeLinkViewModel
    {
        public QRCodeLinkViewModel ()
        {
        }

        public QRCodeLinkViewModel(string qrCodeLink, string qrCodeImage)
        {
            QRCodeLink = qrCodeLink;
            QRCodeImage = qrCodeImage;
        }

        public string QRCodeLink { get; }

        public string QRCodeImage { get; }
    }
}