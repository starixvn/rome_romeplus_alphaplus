namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile
{
    public class CTSCustomerClassCategoryResult
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public IEnumerable<CustomerClassCategory> Detail { get; set; }
    }
}