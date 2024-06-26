using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailViewModel
{
    public WinLossDetailViewModel()
    {
    }

    public WinLossDetailViewModel(
        WinLossDetailQuery message,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossDetailModel> reportData)
            : this(message, productViewModel, reportData)
    {
        IsHistoricReport = message.IsHistoricReport;
        PageHeader = new PageHeaderViewModel(
            Title: GetReportNameByLevel(message.CustLevel),
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel,
            TotalEnteredResults: totalEnteredResults,
            HasNewFeatureNote: true);
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
                isHistoricReport: message.IsHistoricReport);
        }

        CanDrilldown = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
    }

    protected WinLossDetailViewModel(
        ProductDetailDateRangeQuery message,
        ProductMultiSelectionViewModel productViewModel,
        ReportData<WinLossDetailModel> reportData)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevel = message.CustLevel;
        SortPropertyName = message.SortPropertyName;
        DescendingSort = message.DescendingSort;
        IsSetDefaultStackMode = message.IsSetDefaultStackMode;
        FromDate = message.FromDate.ToShortDateWithSlash();
        ToDate = message.ToDate.ToShortDateWithSlash();
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

    public string FromDate { get; }

    public string ToDate { get; }

    public string ViewDataInstructionMessage { get; }

    public PageHeaderViewModel PageHeader { get; protected set; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; protected set; }

    public MonthPickerViewModel MonthPicker { get; }

    public ReportData<WinLossDetailModel> ReportData { get; }

    public bool CanDrilldown { get; protected set; }

    public bool IsGroupHeadline { get; protected set; }

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(IsHistoricReport)] = IsHistoricReport.ToString(),
            [nameof(FromDate)] = FromDate,
            [nameof(ToDate)] = ToDate
        };

    private static string GetReportNameByLevel(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLossDetail
        : custLevel.IsMasterLevel()
            ? Resources.AgentWinLossDetail
            : Resources.MemberWinLossDetail;

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
            KeyValuePair.Create(nameof(IsGroupHeadline), IsGroupHeadline.ToString())
        ]);

        return $"{BetListPath}{queryString}";
    }

    private static string BetListPath = MicroSites.TicketUrl("~/WinLoss/Index");
}