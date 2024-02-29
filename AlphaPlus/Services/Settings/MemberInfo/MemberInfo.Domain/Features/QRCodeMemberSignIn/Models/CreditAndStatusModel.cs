namespace AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models
{
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;

    public class CreditAndStatusModel
    {
        public CreditAndStatusModel()
        {
        }

        public CreditAndStatusModel(
            decimal credit,
            decimal creditMin,
            decimal creditMax,
            string status)
        {
            Credit = credit;
            CreditMin = creditMin;
            CreditMax = creditMax;
            Status = status;
        }

        public decimal Credit { get; set; }

        public decimal CreditMin { get; set; }

        public decimal CreditMax { get; set; }

        public string Status { get; set; }

        public string GivenCreditMustBeBetweenMinMaxMessage
            => string.Format(
                Resources.GivenCreditMustBeBetweenMinMax,
                string.Format("{0:#,0.##}", CreditMin),
                string.Format("{0:#,0.##}", CreditMax));
    }
}
