using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameViewModel<ConsolidationByUsername>(
    ConsolidationQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<ConsolidationCurrency> currencies,
    EnteredResultSport totalEnteredResults,
    IEnumerable<ConsolidationByUsername> reportData) : ConsolidationViewModel<ConsolidationByUsername>(
        message,
        productViewModel,
        currencies,
        totalEnteredResults,
        reportData)
{
}
