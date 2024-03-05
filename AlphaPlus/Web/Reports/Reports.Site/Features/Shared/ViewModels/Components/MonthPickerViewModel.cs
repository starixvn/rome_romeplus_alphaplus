using AlphaPlus.Reports.Features.Shared.Extensions;
using static AlphaPlus.Reports.Site.Resources;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record MonthPickerViewModel(DateTime MinDate, DateTime FromDate, DateTime ToDate, bool IsViewNextYear = false)
{
    public string MonthNames => new[]
    {
        January, February, March, April, May, June, July, August, September, October, November, December
    }.JoinWithComma();
}
