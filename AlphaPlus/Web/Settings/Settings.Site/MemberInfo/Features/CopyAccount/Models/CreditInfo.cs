using AlphaPlus.Settings.MemberInfo.Domain.AppResources;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.Models
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