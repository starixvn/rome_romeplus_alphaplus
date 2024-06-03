using System.Text.Json;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class SportBookInformationQuery : BaseQuery
    {
        public byte RoleId { get; set; }
        public int TabIndex { get; set; }
        public string SiteLabel { get; set; }

        internal string ToJson() => JsonSerializer.Serialize(new
        {
            tabs = BuildTabs(),
            roleId = RoleId,
            site = Site,
            siteLabel = SiteLabel,
        });

        private object BuildTabs()
        {
            return new[]
            {
                BetSettingTab,
                MaxPayoutSettingTab,
                PTSettingTab,
                PTCommissionSettingTab,
                ProductCategorySettingTab,
                WinLossLimitTab
            };
        }

        private object BetSettingTab => new
        {
            name = "BetSetting",
            id = "BetSettingTab",
            isActive = TabIndex == 0,
        };

        private object MaxPayoutSettingTab => new
        {
            name = "MaxPayout",
            id = "MaxPayoutSettingTab",
            url = MicroSites.SettingUrl("~/Customer/GetMaxPayoutSettingInformation?site=" + Site + "&siteLabel=" + SiteLabel),
            isActive = TabIndex == 1,
        };

        private object PTSettingTab => new
        {
            name = "PositionTaking",
            id = "PTSettingTab",
            url = MicroSites.SettingUrl("~/Customer/GetPTSettingInformation?custId=" + CustId + "&site=" + Site),
            isActive = TabIndex == 2,
        };

        private object PTCommissionSettingTab => new
        {
            name = "Commission",
            id = "CommissionHintTab",
            url = MicroSites.SettingUrl("~/Customer/GetCommissionHint?custId=" + CustId + "&site=" + Site),
            isActive = TabIndex == 3,
        };

        private object ProductCategorySettingTab => new
        {
            name = "ProductCategories",
            id = "ProductCategorySettingTab",
            url = MicroSites.SettingUrl("~/Customer/GetProductCategories?custId=" + CustId + "&site=" + Site + "&currency=" + Currency),
            isActive = TabIndex == 4,
        };

        private object WinLossLimitTab => new
        {
            name = "WinLossLimit",
            id = "WinLossLimitTab",
            isActive = TabIndex == 5,
        };
    }
}