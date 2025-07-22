using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using EasyNetQ;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossViewModel : DrillDownableViewModel<WinLossModel>
{
    public WinLossViewModel(
        WinLossQuery message,
        ProductMultiSelectionViewModel productViewModel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossModel> reportData) : base(
            message,
            message.LevelType,
            new PageHeaderViewModel(
                Title: GetTitle(message.CustLevel),
                HasStackIcon: true,
                IsStackMode: message.IsStackMode,
                IsFilterVisible: message.IsFilterVisible,
                BreadcrumbLevel: BuildBreadcrumbViewModel(message),
                TotalEnteredResults: totalEnteredResults),
            reportData,
            message.SortPropertyName,
            message.DescendingSort,
            betListPath: "~/WinLoss/Index",
            cashFeePath: "~/WinLoss/CashFee")
    {
        FromDate = message.FromDate.ToShortDateWithSlash();
        ToDate = message.ToDate.ToShortDateWithSlash();
        CustLevelName = GetCustLevelName(CustLevel);
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

    public bool IsHistoricReport { get; }

    public bool IsSetDefaultStackMode { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public DateRangePickerViewModel DateRangePicker { get; }

    public MonthPickerViewModel MonthPicker { get; }

    public string ViewDataInstructionMessage { get; }

    public string FromDate { get; }

    public string ToDate { get; }

    private static string GetTitle(UserLevel custLevel)
        => custLevel.IsSuperLevel()
        ? Resources.MasterWinLoss
        : custLevel.IsMasterLevel() ? Resources.AgentWinLoss : Resources.MemberWinLoss;

    private static string GetCustLevelName(UserLevel custLevel)
        => custLevel.IsSuperLevel() ? Resources.Super : custLevel.IsMasterLevel() ? Resources.Master : Resources.Agent;

    protected override IDictionary<string, string> AddMoreExcelParams(IDictionary<string, string> queryParams)
    {
        queryParams[nameof(ProductViewModel.UserSelectedProductIds)] = ProductViewModel.UserSelectedProductIds;
        queryParams[nameof(ProductViewModel.UserSelectedResourceIds)] = ProductViewModel.UserSelectedResourceIds;
        queryParams[nameof(IsHistoricReport)] = IsHistoricReport.ToString();
        queryParams[nameof(FromDate)] = FromDate;
        queryParams[nameof(ToDate)] = ToDate;

        return queryParams;
    }

    protected override IDictionary<string, string> AddMoreBetListParams(IDictionary<string, string> queryParams)
    {
        queryParams[nameof(FromDate)] = FromDate;
        queryParams[nameof(ToDate)] = ToDate;
        queryParams["ProductIds"] = ProductViewModel.UserSelectedProductIds;
        queryParams["ResourceIds"] = ProductViewModel.UserSelectedResourceIds;

        return queryParams;
    }

    protected override IDictionary<string, string> AddMoreCashFeeParams(IDictionary<string, string> queryParams)
    {
        queryParams[nameof(FromDate)] = FromDate;
        queryParams[nameof(ToDate)] = ToDate;

        return queryParams;
    }
}