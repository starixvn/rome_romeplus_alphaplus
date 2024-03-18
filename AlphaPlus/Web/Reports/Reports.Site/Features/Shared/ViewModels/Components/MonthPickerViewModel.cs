using AlphaPlus.Reports.Features.Shared.Extensions;
using static AlphaPlus.Reports.Site.Resources;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public record MonthPickerViewModel(DateTime MinDate, DateTime FromDate, DateTime ToDate, bool IsViewNextYear = false)
{
    public DateTime Today { get; } = DateTime.Today;

    public DateTime MaxDate => IsViewNextYear ? Today.AddYears(1).LastDayOfYear() : Today;

    public string MonthNames { get; } = new[]
    {
        January, February, March, April, May, June, July, August, September, October, November, December
    }.JoinWithComma();
}
