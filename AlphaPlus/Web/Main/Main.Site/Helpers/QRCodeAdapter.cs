using QRCoder;
using static QRCoder.PayloadGenerator;

namespace AlphaPlus.Main.Site.Helpers
{
    public class QRCodeAdapter
    {
        public static string Generate(string url)
        {
            var generator = new QRCodeGenerator();
            var qrCodeData = generator.CreateQrCode(new Url(url));
            var graphic = new PngByteQRCode(qrCodeData).GetGraphic(3);

            return $"data:image/png;base64,{Convert.ToBase64String(graphic.ToArray())}";
        }
    }
}