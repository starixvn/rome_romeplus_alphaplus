using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public class DateRangePickerJQueryViewModel(
    DateTime fromDate,
    DateTime toDate,
    DateTime minDate,
    DateTime maxDate,
    bool showShortcuts = true)
{
    public string FromDate { get; } = fromDate.ToShortDateWithSlash();

    public string ToDate { get; } = toDate.ToShortDateWithSlash();

    public DateTime MinDate { get; } = minDate;

    public string MaxDate => maxDate.ToShortDateWithSlash();

    public IEnumerable<DateRangeShortcutViewModel> CustomShortcuts
    {
        get
        {
            if (!showShortcuts)
            {
                return [];
            }

            var customShortcuts = new List<DateRangeShortcutViewModel>();

            var today = DateTime.Today;
            var todayString = today.ToShortDateWithSlash();
            customShortcuts.Add(new DateRangeShortcutViewModel(Resources.Today, [todayString, todayString]));

            var yesterdayString = today.Yesterday().ToShortDateWithSlash();
            customShortcuts.Add(new DateRangeShortcutViewModel(Resources.Yesterday, [yesterdayString, yesterdayString]));

            var thisWeekStartDate = today.StartOfWeek();
            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.ThisWeek,
                [(thisWeekStartDate > MinDate ? thisWeekStartDate : MinDate).ToShortDateWithSlash(), todayString]));

            var lastWeekStartDate = thisWeekStartDate.AddDays(-7);
            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.LastWeek,
                [(lastWeekStartDate > MinDate ? lastWeekStartDate : MinDate).ToShortDateWithSlash(), lastWeekStartDate.AddDays(6).ToShortDateWithSlash()]));

            var thisMonthStartDate = today.FirstDayOfMonth();
            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.ThisMonth,
                [(thisMonthStartDate > MinDate ? thisMonthStartDate : MinDate).ToShortDateWithSlash(), todayString]));

            var lastMonthStartDate = thisMonthStartDate.AddMonths(-1);
            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.LastMonth,
                [(lastMonthStartDate > MinDate ? lastMonthStartDate : MinDate).ToShortDateWithSlash(), thisMonthStartDate.AddDays(-1).ToShortDateWithSlash()]));

            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.SinceLastMonth,
                [(lastMonthStartDate > MinDate ? lastMonthStartDate : MinDate).ToShortDateWithSlash(), todayString]));

            var last3MonthsStartDate = thisMonthStartDate.AddMonths(-3);
            customShortcuts.Add(new DateRangeShortcutViewModel(
                Resources.Last3Months,
                [(last3MonthsStartDate > MinDate ? last3MonthsStartDate : MinDate).ToShortDateWithSlash(), todayString]));

            return customShortcuts;
        }
    }
}