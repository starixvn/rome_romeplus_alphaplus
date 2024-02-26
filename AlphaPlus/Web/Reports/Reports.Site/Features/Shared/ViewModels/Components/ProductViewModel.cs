using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record ProductViewModel(
    IEnumerable<SelectListItem> Products,
    string UserSelectedProductIds,
    string Language);
