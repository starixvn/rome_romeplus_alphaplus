using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossViewModel
{
    public WinLossViewModel(
        WinLossQuery message,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossModel> reportData)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevel = message.CustLevel;
        IsHistoricReport = message.IsHistoricReport;
        var (title, custLevelName, downlineLevelName) = GetTitleCustLevelName(CustLevel);
        CustLevelName = custLevelName;
        DownlineLevelName = downlineLevelName;
        PageHeader = new PageHeaderViewModel(
            Title: title,
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel,
            TotalEnteredResults: totalEnteredResults);
        ProductViewModel = productViewModel;
        if (IsHistoricReport)
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
        ReportData = reportData;
        SortPropertyName = message.SortPropertyName;
        DescendingSort = message.DescendingSort;
        IsSetDefaultStackMode = message.IsSetDefaultStackMode;
    }

    public int CustId { get; }

    public string CustName { get; }

    public UserLevel CustLevel { get; }

    public string CustLevelName { get; }

    public string DownlineLevelName { get; }

    public bool IsHistoricReport { get; }

    public PageHeaderViewModel PageHeader { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; }

    public MonthPickerViewModel MonthPicker { get; }

    public string ViewDataInstructionMessage { get; }

    public ReportData<WinLossModel> ReportData { get; }

    public string SortPropertyName { get; }

    public byte DescendingSort { get; }

    public bool IsSetDefaultStackMode { get; }

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

    private static (string, string, string) GetTitleCustLevelName(UserLevel custLevel)
    {
        string title, custLevelName, downlineLevelName;

        if (custLevel.IsSuperLevel())
        {
            title = Resources.MasterWinLoss;
            custLevelName = Resources.Super;
            downlineLevelName = Resources.Master;
        }
        else if (custLevel.IsMasterLevel())
        {
            title = Resources.AgentWinLoss;
            custLevelName = Resources.Master;
            downlineLevelName = Resources.Agent;
        }
        else
        {
            title = Resources.MemberWinLoss;
            custLevelName = Resources.Agent;
            downlineLevelName = Resources.Member;
        }

        return (title, custLevelName, downlineLevelName);
    }
}