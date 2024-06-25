using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

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
        SortPropertyName = message.SortPropertyName;
        DescendingSort = message.DescendingSort;
        IsSetDefaultStackMode = message.IsSetDefaultStackMode;
        FromDate = message.FromDate.ToShortDateWithSlash();
        ToDate = message.ToDate.ToShortDateWithSlash();
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
    }

    public int CustId { get; }

    public string CustName { get; }

    public UserLevel CustLevel { get; }

    public string CustLevelName { get; }

    public string DownlineLevelName { get; }

    public bool IsHistoricReport { get; }

    public string SortPropertyName { get; }

    public byte DescendingSort { get; }

    public bool IsSetDefaultStackMode { get; }

    public string FromDate { get; }

    public string ToDate { get; }

    public PageHeaderViewModel PageHeader { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; }

    public MonthPickerViewModel MonthPicker { get; }

    public string ViewDataInstructionMessage { get; }

    public ReportData<WinLossModel> ReportData { get; }

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