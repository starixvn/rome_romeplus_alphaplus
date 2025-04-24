using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailViewModel(
    ConsolidationDetailQuery message,
    BreadcrumbViewModel breadcrumbViewModel,
    ProductMultiSelectionViewModel productViewModel,
    EnteredResultSport totalEnteredResults,
    ReportData<WinLossDetailModel> reportData) : DrillDownableViewModel<WinLossDetailModel>(
        message,
        message.LevelType,
        new PageHeaderViewModel(
            Title: Resources.Consolidation,
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbViewModel,
            TotalEnteredResults: totalEnteredResults),
        reportData,
        message.SortPropertyName,
        message.DescendingSort)
{
    private static string BetListPath = MicroSites.TicketUrl("~/WinLoss/Index");

    public bool IsSetDefaultStackMode { get; } = message.IsSetDefaultStackMode;

    public string ReportType { get; } = message.ReportType;

    public string ConsolidationProductIds => ProductViewModel.UserSelectedProductIds;

    public string ConsolidationResourceIds => ProductViewModel.UserSelectedResourceIds;

    public int UserSelectedCurrencyId { get; } = message.UserSelectedCurrencyId;

    public string ConsolidationFromDate => DateRangePicker.FromDate;

    public string ConsolidationToDate => DateRangePicker.ToDate;

    public int ConsolidationUserSelectedDateRangeOption { get; } = message.UserSelectedDateRangeOption;

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public DateRangePickerViewModel DateRangePicker { get; } = new DateRangePickerViewModel(
        message.MinDate,
        DateTime.MaxValue,
        message.FromDate,
        message.ToDate,
        message.UserSelectedDateRangeOption);

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(LevelType)] = LevelType.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };

    public string GetBetListUrl(int custId, string custName)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(custName), custName),
            KeyValuePair.Create(nameof(DateRangePicker.FromDate), DateRangePicker.FromDate),
            KeyValuePair.Create(nameof(DateRangePicker.ToDate), DateRangePicker.ToDate),
            KeyValuePair.Create("ProductIds", ProductViewModel.UserSelectedProductIds),
            KeyValuePair.Create("ResourceIds", ProductViewModel.UserSelectedResourceIds),
            KeyValuePair.Create("IsGroupHeadline", "true")
        ]);

        return $"{BetListPath}{queryString}";
    }
}