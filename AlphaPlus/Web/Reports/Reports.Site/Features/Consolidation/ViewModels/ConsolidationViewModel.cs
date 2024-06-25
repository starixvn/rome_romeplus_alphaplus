using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public abstract class ConsolidationViewModel(
    ConsolidationQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<ConsolidationCurrency> currencies,
    EnteredResultSport totalEnteredResults)
{
    public string ReportType { get; } = message.ReportType;

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: Resources.Consolidation,
        HasStackIcon: true,
        IsStackMode: message.IsStackMode,
        IsFilterVisible: message.IsFilterVisible,
        TotalEnteredResults: totalEnteredResults);

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IEnumerable<SelectListItem> Currencies
        => new SelectListItem[] { new SelectListItem(Resources.AllCurrencies, "0", 0 == message.UserSelectedCurrencyId) }
        .Concat(currencies.Select(c => new SelectListItem(c.CurrencyName, c.Currency.ToString(), c.Currency == message.UserSelectedCurrencyId)));

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption);

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(message.UserSelectedCurrencyId)] = message.UserSelectedCurrencyId.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };

    protected IDictionary<string, string> BuildBaseDrillDownParams(int custId, string custName, byte custLevelId)
        => new Dictionary<string, string>
        {
            [nameof(custId)] = custId.ToString(),
            [nameof(custName)] = custName,
            [nameof(custLevelId)] = custLevelId.ToString(),
            [nameof(ReportType)] = ReportType,
            [nameof(message.UserSelectedCurrencyId)] = message.UserSelectedCurrencyId.ToString(),
            ["ConsolidationProductIds"] = ProductViewModel.UserSelectedProductIds,
            ["ConsolidationResourceIds"] = ProductViewModel.UserSelectedResourceIds,
            ["ConsolidationFromDate"] = DateRangePicker.FromDate,
            ["ConsolidationToDate"] = DateRangePicker.ToDate,
            ["ConsolidationUserSelectedDateRangeOption"] = DateRangePicker.UserSelectedDateRangeOption.ToString(),
        };
}