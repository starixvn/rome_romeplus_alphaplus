namespace AlphaPlus.Main.Site.Features.OTP.Verify
{
    public class VerifyOTPRequest
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string Otp { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public bool IsEnabledTelegramOTP { get; set; }
    }
}