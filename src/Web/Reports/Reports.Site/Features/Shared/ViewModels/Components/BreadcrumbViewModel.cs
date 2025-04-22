namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record BreadcrumbViewModel(IEnumerable<Reports.Features.Shared.Models.Breadcrumb> Breadcrumbs, string BreadcrumbsJson);