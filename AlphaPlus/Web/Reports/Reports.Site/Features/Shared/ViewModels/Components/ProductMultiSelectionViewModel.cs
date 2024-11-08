using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record ProductMultiSelectionViewModel(
    IEnumerable<ProductGroupDTO> Products,
    string UserSelectedProductIds,
    string Language,
    IEnumerable<ProductDetailDTO> ProductDetails = null,
    string UserSelectedResourceIds = null);
