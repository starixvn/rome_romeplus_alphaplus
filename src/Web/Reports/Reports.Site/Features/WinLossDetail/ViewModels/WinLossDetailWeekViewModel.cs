
namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public record WinLossDetailWeekViewModel(
    string FromDate,
    string ToDate,
    string Display,
    bool IsEnable,
    bool IsActive);
