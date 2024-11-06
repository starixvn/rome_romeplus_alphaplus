using AlphaPlus.Shared.AppResources.Main;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.OTP
{
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