using System.Text.Json;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class SportBookInformationQuery : BaseQuery
    {
        public byte RoleId { get; set; }
        public int TabIndex { get; set; }
        public string SiteLabel { get; set; }

        public bool IsCashMember { get; set; }

        internal string ToJson() => JsonSerializer.Serialize(new
        {
            tabs = BuildTabs(),
            roleId = RoleId,
            site = Site,
            siteLabel = SiteLabel,
            isCashMember = IsCashMember
        });

        private object BuildTabs()
        {
            var tabs = new List<object>
            {
                BetSettingTab,
                MaxPayoutSettingTab,
                PTSettingTab,
                PTCommissionSettingTab,
                ProductCategorySettingTab,
                WinLossLimitTab,
                PotentialWinLimitTab
            };

            return tabs;
        }

        private object BetSettingTab => new
        {
            name = "BetSetting",
            id = "BetSettingTab",
            isActive = TabIndex == 0,
            url = MicroSites.SettingUrl("~/Customer/GetBetLimitInformation?site=" + Site + "&siteLabel=" + SiteLabel),
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

        private object PotentialWinLimitTab => new
        {
            name = "PotentialWinLimit",
            id = "PotentialWinLimitSettingTab",
            isActive = TabIndex == 6,
        };
    }
}