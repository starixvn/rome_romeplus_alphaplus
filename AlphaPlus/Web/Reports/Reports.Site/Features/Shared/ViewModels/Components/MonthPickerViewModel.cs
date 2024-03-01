using static AlphaPlus.Reports.Site.Resources;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record MonthPickerViewModel(DateTime MinDate, DateTime FromDate, DateTime ToDate)
{
    public string[] MonthNames => [
        January, February, March, April, May, June, July, August, September, October, November, December
    ];
}
