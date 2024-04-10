using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByProductViewModel(
    ConsolidationQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<ConsolidationCurrency> currencies,
    EnteredResultSport totalEnteredResults,
    IEnumerable<ConsolidationByGroup> reportData) : ConsolidationViewModel(
        message,
        productViewModel,
        currencies,
        totalEnteredResults)
{
    public IEnumerable<ConsolidationByGroup> ReportData { get; } = reportData;

    public IDictionary<string, string> BuildDrillDownParams(int custId, string custName, byte custLevelId, string productId)
    {
        var drillDownParams = BuildBaseDrillDownParams(custId, custName, custLevelId);

        drillDownParams.Add("UserSelectedProductIds", productId);
        drillDownParams.Add(nameof(ProductViewModel.UserSelectedResourceIds), ProductViewModel.UserSelectedResourceIds);
        drillDownParams.Add(nameof(DateRangePicker.FromDate), DateRangePicker.FromDate);
        drillDownParams.Add(nameof(DateRangePicker.ToDate), DateRangePicker.ToDate);

        return drillDownParams;
    }
}