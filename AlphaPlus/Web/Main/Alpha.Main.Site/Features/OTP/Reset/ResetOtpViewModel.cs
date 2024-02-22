namespace Alpha.Main.Site.Features.OTP
{
    using Alpha.Main.AppResources;
    using AlphaPlus.Core.Models;

    public class ResetOtpViewModel : ResetOtpQuery
    {
        public string ResetTitle
        {
            get
            {
                return string.Format(Security.RemoveOtpForCust, Username);
            }
        }

        public Result Result { get; set; }
    }
}