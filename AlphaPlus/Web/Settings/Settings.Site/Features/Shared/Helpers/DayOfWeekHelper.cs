using AlphaPlus.Shared.AppResources.Settings.Transfer;

namespace AlphaPlus.Settings.Site.Features.Shared.Helpers;

public static class DayOfWeekHelper
{
    private const int TotalDayOfWeek = 7;
    private static readonly string[] DaysOfWeek =
    [
        Resources.Sun,
        Resources.Mon,
        Resources.Tue,
        Resources.Wed,
        Resources.Thurs,
        Resources.Fri,
        Resources.Sat
    ];

    public static string BuildDays(string scheduleWeekDays)
    {
        if (!string.IsNullOrWhiteSpace(scheduleWeekDays))
        {
            var days = scheduleWeekDays.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return days.Length == TotalDayOfWeek
                ? Resources.Daily
                : string.Join('-', days.Select(d => DaysOfWeek[int.Parse(d) - 1]));
        }

        return string.Empty;
    }
}