using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

public record TabViewModel
{
    public string SelectedTabName { get; init; }
    public string HandbookUrl { get; init; }

    public TabViewModel(string selectedTabName)
    {
        SelectedTabName = selectedTabName;
        HandbookUrl = AppSettings.IsSiteLabel789y ? MicroSites.FeatureUrl("~/StaticMessage/Help?id=226060") : MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=member-win-loss-limit");
    }
}