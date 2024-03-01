using AlphaPlus.Reports.Features.EnteredResult;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record PageHeaderViewModel(
    string Title,
    bool HasStackIcon,
    bool HasHideFilterIcon,
    bool IsStackMode,
    bool IsFilterVisible,
    // IEnumerable<Breadcrumb> BreadcrumbLevel = null,
    // string BreadcrumbLevelJson = null,
    // IEnumerable<string> BreadcrumbParamByLevel = null,
    // string RequestQuery = null,
    BreadcrumbLevelViewModel BreadcrumbLevelViewModel = null,
    EnteredResultSport TotalEnteredResults = null,
    bool HasNewFeatureNote = false)
{
    public bool IsShowActionIcons { get; } = HasStackIcon || HasHideFilterIcon;

    //public BreadcrumbLevelViewModel BreadcrumbLevelViewModel { get; } = new BreadcrumbLevelViewModel(BreadcrumbLevel, BreadcrumbLevelJson, BreadcrumbParamByLevel, RequestQuery);
}