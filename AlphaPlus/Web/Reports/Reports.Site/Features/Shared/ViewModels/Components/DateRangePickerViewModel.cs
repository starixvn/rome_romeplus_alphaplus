using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public class DateRangePickerViewModel
{
    private const string TodayOptionValue = "0";
    private const string YesterdayOptionValue = "1";
    private const string ThisWeekOptionValue = "2";
    private const string LastWeekOptionValue = "3";
    private const string ThisMonthOptionValue = "4";
    private const string LastMonthOptionValue = "5";
    private const string SinceLastMonthOptionValue = "6";
    private const string SpecificDateOptionValue = "7";
    private const string OlderThan3MonthsOptionValue = "8";

    public DateRangePickerViewModel(
        DateTime minDate,
        DateTime maxDate,
        DateTime fromDate,
        DateTime toDate,
        int userSelectedDateRangeOption,
        bool isRenderSubmitButton = true,
        IList<string> optionValues = null,
        bool isSupportHistoricReport = false,
        bool isHistoricReport = false)
    {
        MinDate = minDate;
        MaxDate = maxDate;
        FromDate = fromDate;
        ToDate = toDate;
        UserSelectedDateRangeOption = userSelectedDateRangeOption;
        //// TODO: ricky will refactor here
        var rangeOptions = BuildRangeOptions(userSelectedDateRangeOption, isSupportHistoricReport, isHistoricReport);
        RangeOptions = optionValues != null ? rangeOptions.Where(item => optionValues.Contains(item.Value)) : rangeOptions;

        SelectedRangeOption = RangeOptions.FirstOrDefault(item => item.Value == userSelectedDateRangeOption.ToString());

        IsRenderSubmitButton = isRenderSubmitButton;
    }

    public DateTime MinDate { get; }

    public DateTime MaxDate { get; }

    public DateTime FromDate { get; }

    public DateTime ToDate { get; }

    public int UserSelectedDateRangeOption { get; }

    public bool IsRenderSubmitButton { get; }

    public IEnumerable<SelectListItem> RangeOptions { get; }

    public SelectListItem SelectedRangeOption { get; private set; }

    private IEnumerable<SelectListItem> BuildRangeOptions(int userSelectedDateRangeOption, bool isSupportHistoricReport = false, bool isHistoricReport = false)
    {
        // Cannot use static here because Localization.
        var result = new List<SelectListItem>
        {
            new SelectListItem { Text = Resources.Today, Value = TodayOptionValue, Selected = TodayOptionValue == userSelectedDateRangeOption.ToString() },
            new SelectListItem { Text = Resources.Yesterday, Value = YesterdayOptionValue, Selected = YesterdayOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.ThisWeek, Value = ThisWeekOptionValue, Selected = ThisWeekOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.LastWeek, Value = LastWeekOptionValue, Selected = LastWeekOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.ThisMonth, Value = ThisMonthOptionValue, Selected = ThisMonthOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.LastMonth, Value = LastMonthOptionValue, Selected = LastMonthOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.SinceLastMonth, Value = SinceLastMonthOptionValue, Selected = SinceLastMonthOptionValue == userSelectedDateRangeOption.ToString()},
            new SelectListItem { Text = Resources.SpecificDate, Value = SpecificDateOptionValue, Selected = SpecificDateOptionValue == userSelectedDateRangeOption.ToString()}
        };

        if (isSupportHistoricReport && !isHistoricReport)
        {
            result.Add(new SelectListItem { Text = Resources.OlderThan3Months, Value = OlderThan3MonthsOptionValue, Selected = OlderThan3MonthsOptionValue == userSelectedDateRangeOption.ToString() });
        }

        return result;
    }
}