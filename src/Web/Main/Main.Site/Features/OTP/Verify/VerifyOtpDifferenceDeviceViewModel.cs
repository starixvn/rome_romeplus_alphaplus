namespace AlphaPlus.Main.Site.Features.OTP
{
    public class VerifyOtpDifferenceDeviceViewModel
    {
        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }

        public bool IsShowGenerateOTPButton { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }
        
        public int ProfileImageId { get; set; }
    }
}