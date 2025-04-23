using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record BreadcrumbViewModel(IEnumerable<Breadcrumb> Breadcrumbs, string BreadcrumbsJson);