using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public record WinLossDetailGetDataViewModel(
    byte CustLevelId,
    EnteredResultSport TotalEnteredResults,
    ReportData<WinLossDetailModel> ReportData,
    IEnumerable<WinLossDetailWeekViewModel> Weeks,
    IEnumerable<Breadcrumb> Breadcrumbs);