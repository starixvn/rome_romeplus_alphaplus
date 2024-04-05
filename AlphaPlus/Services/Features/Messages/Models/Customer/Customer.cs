namespace AlphaPlus.Features.Messages.Models.Customer
{
    using AlphaPlus.Core.Configurations;

    public class Customer
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public byte RoleId { get; set; }

        public byte CurrencyId { get; set; }

        public int ARecommendId { get; set; }

        public int MRecommendId { get; set; }

        public int SRecommendId { get; set; }

        public string Site { get; set; }

        public bool IsIMBC => SiteLabel.Equals("MBC");

        public bool IsAllDayBalTransfer { get; set; }

        public string System => ConfigurationManager.GetValue<string>($"System:{SiteLabel}");

        public bool Enabled { get; set; }

        public string SiteLabel { get; set; }
    }
}