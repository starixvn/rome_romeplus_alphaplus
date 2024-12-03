using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByDateViewModel(
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

    public IDictionary<string, string> BuildDrillDownParams(int custId, string custName, byte custLevelId, string date)
    {
        var drillDownParams = BuildBaseDrillDownParams(custId, custName, custLevelId, fromDate: date, toDate: date);
        drillDownParams[nameof(DateRangePicker.UserSelectedDateRangeOption)] = DateRangePickerViewModel.SpecificDateOptionValue.ToString();

        return drillDownParams;
    }

    public string BuildBetListUrl(int custId, string custName, string date)
        => BuildBaseBetListParams(custId, custName, fromDate: date, toDate: date);
}