using AlphaPlus.Reports.Features.EnteredResult;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record PageHeaderViewModel(
    string Title,
    bool HasStackIcon = false,
    bool HasHideFilterIcon = true,
    bool IsStackMode = false,
    bool IsFilterVisible = true,
    BreadcrumbViewModel BreadcrumbLevel = null,
    EnteredResultSport TotalEnteredResults = null)
{
    public bool IsShowActionIcons { get; } = HasStackIcon || HasHideFilterIcon;
}