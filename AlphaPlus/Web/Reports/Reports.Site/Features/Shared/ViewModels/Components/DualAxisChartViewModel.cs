
namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record DualAxisChartViewModel(
    string Title,
    string Labels,
    string FirstDataLabel,
    string SecondDataLabel,
    string FirstData,
    string SecondData,
    string CurrencyName);