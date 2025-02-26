using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior
{
    public class MemberBettingBehaviorViewModel
    {
        public MemberBettingBehaviorViewModel(bool isDarkMode, string language)
        {
            IsDarkMode = isDarkMode;
            Language = language;
        }

        public IEnumerable<SelectListItem> DateRange
        {
            get
            {
                foreach (var item in MemberStatisticDateRange.Items)
                {
                    yield return new SelectListItem { Text = Resources.ResourceManager.GetString(item.Key), Value = item.Value.ToString() };
                }
            }
        }

        public bool IsDarkMode { get; }

        public DateTime EndDate => DateTime.Now.AddDays(-2);

        public string UserGuideLink
        {
            get
            {
                if (AppSettings.SupportedNewHandbook)
                {
                    return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=member-betting-behavior");
                }

                return AppSettings.IsSiteLabel789y
                   ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=204089")
                   : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=204089");
            }
        }

        public string Language { get; }
    }
}