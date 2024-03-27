namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record ProductMultiSelectionViewModel(
    IEnumerable<ProductGroupViewModel> Products,
    string UserSelectedProductIds,
    string Language,
    IEnumerable<ProductDetailViewModel> ProductDetails = null,
    string UserSelectedResourceIds = null);
