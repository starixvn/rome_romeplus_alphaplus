using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public abstract class ConsolidationViewModel(
    ConsolidationQuery message,
    ProductMultiSelectionViewModel productViewModel,
    IEnumerable<ConsolidationCurrency> currencies,
    EnteredResultSport totalEnteredResults)
{
    private static string BetListPath = MicroSites.TicketUrl("~/WinLoss/Index");

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
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate,
            [nameof(message.ReportType)] = message.ReportType
        };

    protected IDictionary<string, string> BuildBaseDrillDownParams(int custId, string custName, byte custLevelId, string productIds = null, string fromDate = null, string toDate = null)
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
            [nameof(ProductViewModel.UserSelectedProductIds)] = productIds ?? ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(DateRangePicker.FromDate)] = fromDate ?? DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = toDate ?? DateRangePicker.ToDate,
            [nameof(DateRangePicker.UserSelectedDateRangeOption)] = DateRangePicker.UserSelectedDateRangeOption.ToString()
        };

    protected string BuildBaseBetListParams(int custId, string custName, string productIds = null, string fromDate = null, string toDate = null)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(custName), custName),
            KeyValuePair.Create(nameof(DateRangePicker.FromDate), fromDate ?? DateRangePicker.FromDate),
            KeyValuePair.Create(nameof(DateRangePicker.ToDate), toDate ?? DateRangePicker.ToDate),
            KeyValuePair.Create("ProductIds", productIds ?? ProductViewModel.UserSelectedProductIds),
            KeyValuePair.Create("ResourceIds", ProductViewModel.UserSelectedResourceIds),
            KeyValuePair.Create("IsGroupHeadline", "true")
        ]);

        return $"{BetListPath}{queryString}";
    }
}