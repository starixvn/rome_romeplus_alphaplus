using Alpha.Main.Site.Helpers;
using Alpha.Main.Models.Otp;
using Alpha.Main.Site.Features.TelegramChatBot;

namespace Alpha.Main.Site.Features.OTP
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