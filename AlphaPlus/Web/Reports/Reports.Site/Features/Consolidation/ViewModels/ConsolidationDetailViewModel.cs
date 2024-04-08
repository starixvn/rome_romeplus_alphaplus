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
        ReportData<WinLossDetailModel> reportData)
            : base(message, productViewModel, breadcrumbLevel, totalEnteredResults, reportData)
    {
        DateRangePicker = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);
    }

    public int UserSelectedCurrencyId { get; }

    public string ConsolidationProductIds { get; }

    public string ConsolidationResourceIds { get; }

    public DateTime ConsolidationFromDate { get; }

    public DateTime ConsolidationToDate { get; }

    public int ConsolidationUserSelectedDateRangeOption { get; }
}