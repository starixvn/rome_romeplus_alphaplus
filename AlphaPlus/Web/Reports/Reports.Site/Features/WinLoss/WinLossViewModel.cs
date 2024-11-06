using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossViewModel : DrillDownableViewModel<WinLossModel>
{
    private static string BetListPath = MicroSites.TicketUrl("~/WinLoss/Index");

    public WinLossViewModel(
        WinLossQuery message,
        ProductMultiSelectionViewModel productViewModel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossModel> reportData) : base(
            message,
            new PageHeaderViewModel(
                Title: GetTitle(message.CustLevel),
                HasStackIcon: true,
                IsStackMode: message.IsStackMode,
                IsFilterVisible: message.IsFilterVisible,
                BreadcrumbLevel: BuildBreadcrumbViewModel(message),
                TotalEnteredResults: totalEnteredResults),
            reportData,
            message.SortPropertyName,
            message.DescendingSort)
    {
        var (custLevelName, downlineLevelName) = GetCustLevelAndDownlineLevelName(CustLevel);
        CustLevelName = custLevelName;
        DownlineLevelName = downlineLevelName;
        IsSetDefaultStackMode = message.IsSetDefaultStackMode;
        ProductViewModel = productViewModel;
        IsHistoricReport = message.IsHistoricReport;
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
    }

    public string CustLevelName { get; }

    public string DownlineLevelName { get; }

    public bool IsHistoricReport { get; }

    public bool IsSetDefaultStackMode { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; }

    public MonthPickerViewModel MonthPicker { get; }

    public string ViewDataInstructionMessage { get; }

    private static string GetTitle(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLoss
        : custLevel.IsMasterLevel() ? Resources.AgentWinLoss : Resources.MemberWinLoss;

    private static (string, string) GetCustLevelAndDownlineLevelName(UserLevel custLevel)
    {
        string custLevelName, downlineLevelName;

        if (custLevel.IsSuperLevel())
        {
            custLevelName = Resources.Super;
            downlineLevelName = Resources.Master;
        }
        else if (custLevel.IsMasterLevel())
        {
            custLevelName = Resources.Master;
            downlineLevelName = Resources.Agent;
        }
        else
        {
            custLevelName = Resources.Agent;
            downlineLevelName = Resources.Member;
        }

        return (custLevelName, downlineLevelName);
    }

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds,
            [nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds,
            [nameof(IsHistoricReport)] = IsHistoricReport.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate,
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
        ]);

        return $"{BetListPath}{queryString}";
    }
}