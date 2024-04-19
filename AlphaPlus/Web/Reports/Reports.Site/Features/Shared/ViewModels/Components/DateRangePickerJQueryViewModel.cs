using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public class DateRangePickerJQueryViewModel(
    DateTime fromDate,
    DateTime toDate,
    DateTime minDate,
    DateTime maxDate,
    string language,
    bool showShortcuts = true,
    int[] customShortcuts = null)
{
    public const int TodayOptionValue = 0;
    public const int YesterdayOptionValue = 1;
    public const int ThisWeekOptionValue = 2;
    public const int LastWeekOptionValue = 3;
    public const int ThisMonthOptionValue = 4;
    public const int LastMonthOptionValue = 5;
    public const int SinceLastMonthOptionValue = 6;
    public const int Last3MonthsOptionValue = 7;

    public string FromDate { get; } = fromDate.ToShortDateWithSlash();

    public string ToDate { get; } = toDate.ToShortDateWithSlash();

    public string MinDate { get; } = minDate.ToShortDateWithSlash();

    public string MaxDate => maxDate.ToShortDateWithSlash();

    public string Language { get; } = language;

    public bool ShowShortcuts { get; } = showShortcuts;

    public int[] CustomShortcuts { get; }
        = showShortcuts && customShortcuts == null
            ? [
                TodayOptionValue,
                YesterdayOptionValue,
                ThisWeekOptionValue,
                LastWeekOptionValue,
                ThisMonthOptionValue,
                LastMonthOptionValue,
                SinceLastMonthOptionValue,
                Last3MonthsOptionValue
            ]
            : customShortcuts;

    public string Today => DateTime.Today.ToShortDateWithSlash();
}