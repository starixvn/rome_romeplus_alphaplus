using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameViewModel(
    ConsolidationQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<ConsolidationCurrency> currencies,
    EnteredResultSport totalEnteredResults,
    IEnumerable<ConsolidationByUsername> reportData) : ConsolidationViewModel(
        message,
        productViewModel,
        currencies,
        totalEnteredResults)
{
    public IEnumerable<ConsolidationByUsername> ReportData { get; } = reportData;

    public IDictionary<string, string> BuildDrillDownParams(int custId, string custName, byte custLevelId)
        => BuildBaseDrillDownParams(custId, custName, custLevelId);

    public string BuildBetListUrl(int custId, string custName) => BuildBaseBetListParams(custId, custName);
}
