using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.Main.Site.Features.TelegramChatBot;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class OtpViewModel
    {
        public CustomerOtp CustomerOtp { get; set; }

        public bool IsAlreadySetup { get; set; }

        public bool IsOfferProtectedMode { get; set; }

        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }

        public bool EnableSecurityCode { get; set; }

        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}