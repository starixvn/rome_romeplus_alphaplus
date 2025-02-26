
using AlphaPlus.BetLists.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;

public record ProductMultiSelectionViewModel(
    IEnumerable<ProductGroupDTO> Products,
    string UserSelectedProductIds,
    string Language,
    IEnumerable<ProductDetailDTO> ProductDetails = null,
    string UserSelectedResourceIds = null);
