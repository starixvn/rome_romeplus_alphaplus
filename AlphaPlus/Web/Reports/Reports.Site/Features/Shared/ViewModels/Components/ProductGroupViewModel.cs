namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record ProductGroupViewModel(
    int Id,
    string Name,
    bool Checked,
    IEnumerable<ProductGroupViewModel> Products);
