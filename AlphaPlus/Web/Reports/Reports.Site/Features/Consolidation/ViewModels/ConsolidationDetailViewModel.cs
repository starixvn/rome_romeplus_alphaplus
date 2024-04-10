using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Reports.Site.Features.WinLossDetail;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailViewModel : WinLossDetailViewModel
{
    public ConsolidationDetailViewModel(
        ConsolidationDetailQuery message,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        EnteredResultSport totalEnteredResults,
        ReportData<WinLossDetailModel> reportData) : base(message, productViewModel, reportData)
    {
        PageHeader = new PageHeaderViewModel(
            Title: Resources.Consolidation,
            HasStackIcon: true,
            IsStackMode: message.IsStackMode,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel,
            TotalEnteredResults: totalEnteredResults);
        DateRangePicker = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);
        CanDrilldown = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

        ReportType = message.ReportType;
        ConsolidationProductIds = ProductViewModel.UserSelectedProductIds;
        ConsolidationResourceIds = ProductViewModel.UserSelectedResourceIds;
        UserSelectedCurrencyId = message.UserSelectedCurrencyId;
        ConsolidationFromDate = DateRangePicker.FromDate;
        ConsolidationToDate = DateRangePicker.ToDate;
        ConsolidationUserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
    }

    public string ReportType { get; }

    public string ConsolidationProductIds { get; }

    public string ConsolidationResourceIds { get; }

    public int UserSelectedCurrencyId { get; }

    public string ConsolidationFromDate { get; }

    public string ConsolidationToDate { get; }

    public int ConsolidationUserSelectedDateRangeOption { get; }
}