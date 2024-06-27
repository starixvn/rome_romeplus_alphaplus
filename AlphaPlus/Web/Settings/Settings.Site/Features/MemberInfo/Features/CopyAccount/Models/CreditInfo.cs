using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CopyAccount.Models
{
    public class CreditInfo
    {
        public decimal Credit { get; set; }

        public decimal MaxCredit { get; set; }

        public string GivenCreditMustBeBetweenMinMaxMessage
            => string.Format(
                Resources.GivenCreditMustBeBetweenMinMax,
                string.Format("{0:#,0.##}", 0),
                string.Format("{0:#,0.##}", MaxCredit));
    }
}