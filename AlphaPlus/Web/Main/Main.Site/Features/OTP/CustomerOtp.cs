using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class CustomerOtp
    {
        public int CustId { get; set; }

        public string OtpUsername { get; set; }

        public int SubAccId { get; set; }

        public string SecretKey { get; set; }

        public string OTPUrl { get; set; }

        public bool Enabled { get; set; }

        public string Otp { get; set; }

        public Device Device { get; set; }

        public string Language { get; set; }

        public bool IsEnabledTelegramOTP { get; set; }
    }
}