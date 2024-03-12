using AlphaPlus.Reports.Features.EnteredResult;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record PageHeaderViewModel(
    string Title,
    bool HasStackIcon = false,
    bool HasHideFilterIcon = true,
    bool IsStackMode = false,
    bool IsFilterVisible = true,
    // IEnumerable<Breadcrumb> BreadcrumbLevel = null,
    // string BreadcrumbLevelJson = null,
    // IEnumerable<string> BreadcrumbParamByLevel = null,
    // string RequestQuery = null,
    BreadcrumbLevelViewModel BreadcrumbLevel = null,
    EnteredResultSport TotalEnteredResults = null,
    bool HasNewFeatureNote = false)
{
    public bool IsShowActionIcons { get; } = HasStackIcon || HasHideFilterIcon;

    //public BreadcrumbLevelViewModel BreadcrumbLevelViewModel { get; } = new BreadcrumbLevelViewModel(BreadcrumbLevel, BreadcrumbLevelJson, BreadcrumbParamByLevel, RequestQuery);
}