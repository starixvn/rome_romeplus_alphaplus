using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailViewModel
{
    public WinLossDetailViewModel(
        WinLossDetailQuery message,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossDetailModel> reportData)
            : this((ProductDetailDateRangeQuery)message, productViewModel, breadcrumbLevel, totalEnteredResults, reportData)
    {
        if (message.IsHistoricReport)
        {
            MonthPicker = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());
        }
        else
        {
            DateRangePicker = new DateRangePickerViewModel(
                message.MinDate,
                DateTime.MaxValue,
                message.FromDate,
                message.ToDate,
                message.UserSelectedDateRangeOption,
                isSupportHistoricReport: true,
                isHistoricReport: IsHistoricReport);
        }

        CanDrilldown = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
    }

    protected WinLossDetailViewModel(
        ProductDetailDateRangeQuery message,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossDetailModel> reportData)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevel = message.CustLevel;
        SortPropertyName = message.SortPropertyName;
        DescendingSort = message.DescendingSort;
        IsSetDefaultStackMode = message.IsSetDefaultStackMode;
        PageHeader = new PageHeaderViewModel(
            Title: Resources.Consolidation,
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel,
            TotalEnteredResults: totalEnteredResults);
        ProductViewModel = productViewModel;
        ReportData = reportData;
    }

    public int CustId { get; }

    public string CustName { get; }

    public UserLevel CustLevel { get; }

    public string SortPropertyName { get; }

    public byte DescendingSort { get; }

    public bool IsSetDefaultStackMode { get; }

    public bool IsHistoricReport { get; }

    public string ViewDataInstructionMessage { get; }

    public PageHeaderViewModel PageHeader { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; protected set; }

    public MonthPickerViewModel MonthPicker { get; }

    public ReportData<WinLossDetailModel> ReportData { get; }

    public bool CanDrilldown { get; }

    public IDictionary<string, string> GetExcelParams()
    {
        var excelParam = new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(IsHistoricReport)] = IsHistoricReport.ToString()
        };

        string fromDate, toDate;
        if (IsHistoricReport)
        {
            fromDate = MonthPicker.FromDate.ToShortDateWithSlash();
            toDate = MonthPicker.ToDate.ToShortDateWithSlash();
        }
        else
        {
            fromDate = DateRangePicker.FromDate;
            toDate = DateRangePicker.ToDate;
        }

        excelParam.Add(nameof(fromDate), fromDate);
        excelParam.Add(nameof(toDate), toDate);

        return excelParam;
    }
}