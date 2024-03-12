using AlphaPlus.Reports.Features.Shared.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public class DateRangePickerViewModel
{
    public const int TodayOptionValue = 0;
    public const int YesterdayOptionValue = 1;
    public const int ThisWeekOptionValue = 2;
    public const int LastWeekOptionValue = 3;
    public const int ThisMonthOptionValue = 4;
    public const int LastMonthOptionValue = 5;
    public const int SinceLastMonthOptionValue = 6;
    public const int SpecificDateOptionValue = 7;
    public const int OlderThan3MonthsOptionValue = 8;

    public DateRangePickerViewModel(
        DateTime minDate,
        DateTime maxDate,
        DateTime fromDate,
        DateTime toDate,
        int userSelectedDateRangeOption,
        bool isRenderSubmitButton = true,
        IEnumerable<int> optionValues = null,
        bool isSupportHistoricReport = false,
        bool isHistoricReport = false)
    {
        Today = DateTime.Today.ToShortDateWithSlash();
        MinDate = minDate.ToShortDateWithSlash();
        MaxDate = maxDate.ToShortDateWithSlash();
        FromDate = fromDate.ToShortDateWithSlash();
        ToDate = toDate.ToShortDateWithSlash();
        UserSelectedDateRangeOption = userSelectedDateRangeOption;
        //// TODO: ricky will refactor here
        var rangeOptions = BuildRangeOptions(isSupportHistoricReport, isHistoricReport);
        RangeOptions = optionValues != null ? rangeOptions.Where(item => optionValues.Contains(int.Parse(item.Value))) : rangeOptions;
        IsRenderSubmitButton = isRenderSubmitButton;
    }

    public string Today { get; }

    public string MinDate { get; }

    public string MaxDate { get; }

    public string FromDate { get; }

    public string ToDate { get; }

    public int UserSelectedDateRangeOption { get; }

    public bool IsRenderSubmitButton { get; }

    public IEnumerable<SelectListItem> RangeOptions { get; }

    public SelectListItem SelectedRangeOption { get; private set; }

    private IEnumerable<SelectListItem> BuildRangeOptions(bool isSupportHistoricReport = false, bool isHistoricReport = false)
    {
        // Cannot use static here because Localization.
        var selectListItems = new List<SelectListItem>
        {
            BuildSelectListItem(Resources.Today, TodayOptionValue),
            BuildSelectListItem(Resources.Yesterday, YesterdayOptionValue),
            BuildSelectListItem(Resources.ThisWeek, ThisWeekOptionValue),
            BuildSelectListItem(Resources.LastWeek, LastWeekOptionValue),
            BuildSelectListItem(Resources.ThisMonth, ThisMonthOptionValue),
            BuildSelectListItem(Resources.LastMonth, LastMonthOptionValue),
            BuildSelectListItem(Resources.SinceLastMonth, SinceLastMonthOptionValue),
            BuildSelectListItem(Resources.SpecificDate, SpecificDateOptionValue)
        };

        if (isSupportHistoricReport && !isHistoricReport)
        {
            selectListItems.Add(BuildSelectListItem(Resources.OlderThan3Months, OlderThan3MonthsOptionValue));
        }

        return selectListItems;
    }

    private SelectListItem BuildSelectListItem(string text, int value)
    {
        var selected = value == UserSelectedDateRangeOption;
        var selectListItem = new SelectListItem(text, value.ToString(), selected);

        if (selected)
        {
            SelectedRangeOption = selectListItem;
        }

        return selectListItem;
    }
}