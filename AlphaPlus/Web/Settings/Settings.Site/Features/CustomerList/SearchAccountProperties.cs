using AlphaPlus.Products.Configurations;

namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    public class SearchAccountProperties : IExtendProductProperty
    {
        public SearchAccountProperties(bool setting, bool status, bool statusOnProductLevel)
        {
            Setting = setting;
            Status = status;
            StatusOnProductLevel = statusOnProductLevel;
        }

        public bool Setting { get; }

        public bool Status { get; }

        public bool StatusOnProductLevel { get; }
    }
}