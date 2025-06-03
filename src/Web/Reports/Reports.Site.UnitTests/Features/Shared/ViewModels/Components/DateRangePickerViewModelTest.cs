using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.ViewModels.Components;

[Trait("Category", "Shared")]
public class DateRangePickerViewModelTest
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;
        var isRenderSubmitButton = true;

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption,
            isRenderSubmitButton);

        // Assert
        Assert.Equal(SystemTime.Today.ToShortDateWithSlash(), viewModel.Today);
        Assert.Equal(minDate.ToShortDateWithSlash(), viewModel.MinDate);
        Assert.Equal(maxDate.ToShortDateWithSlash(), viewModel.MaxDate);
        Assert.Equal(fromDate.ToShortDateWithSlash(), viewModel.FromDate);
        Assert.Equal(toDate.ToShortDateWithSlash(), viewModel.ToDate);
        Assert.Equal(userSelectedDateRangeOption, viewModel.UserSelectedDateRangeOption);
        Assert.Equal(isRenderSubmitButton, viewModel.IsRenderSubmitButton);
    }

    [Fact]
    public void Constructor_ShouldBuildRangeOptionsCorrectly()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption);

        // Assert
        Assert.NotNull(viewModel.RangeOptions);
        Assert.Equal(8, viewModel.RangeOptions.Count());

        // Verify all standard options are present
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.TodayOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.YesterdayOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.ThisWeekOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.LastWeekOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.ThisMonthOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.LastMonthOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.SinceLastMonthOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.SpecificDateOptionValue.ToString());
    }

    [Fact]
    public void Constructor_WithSupportHistoricReport_ShouldIncludeOlderThan3MonthsOption()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;
        var isSupportHistoricReport = true;
        var isHistoricReport = false;

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption,
            true,
            null,
            isSupportHistoricReport,
            isHistoricReport);

        // Assert
        Assert.NotNull(viewModel.RangeOptions);
        Assert.Equal(9, viewModel.RangeOptions.Count());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.OlderThan3MonthsOptionValue.ToString());
    }

    [Fact]
    public void Constructor_WithSupportHistoricReportAndIsHistoricReport_ShouldNotIncludeOlderThan3MonthsOption()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;
        var isSupportHistoricReport = true;
        var isHistoricReport = true;

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption,
            true,
            null,
            isSupportHistoricReport,
            isHistoricReport);

        // Assert
        Assert.NotNull(viewModel.RangeOptions);
        Assert.Equal(8, viewModel.RangeOptions.Count());
        Assert.DoesNotContain(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.OlderThan3MonthsOptionValue.ToString());
    }

    [Fact]
    public void Constructor_WithSelectedOptionValue_ShouldSetSelectedRangeOption()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption);

        // Assert
        Assert.NotNull(viewModel.SelectedRangeOption);
        Assert.Equal(userSelectedDateRangeOption.ToString(), viewModel.SelectedRangeOption.Value);
        Assert.Equal(Resources.ThisMonth, viewModel.SelectedRangeOption.Text);
        Assert.True(viewModel.SelectedRangeOption.Selected);
    }

    [Fact]
    public void Constructor_WithOptionValues_ShouldFilterRangeOptions()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 5, 1);
        var toDate = new DateTime(2023, 5, 31);
        var userSelectedDateRangeOption = DateRangePickerViewModel.ThisMonthOptionValue;
        var optionValues = new[] {
            DateRangePickerViewModel.ThisMonthOptionValue,
            DateRangePickerViewModel.LastMonthOptionValue
        };

        // Act
        var viewModel = new DateRangePickerViewModel(
            minDate,
            maxDate,
            fromDate,
            toDate,
            userSelectedDateRangeOption,
            true,
            optionValues);

        // Assert
        Assert.NotNull(viewModel.RangeOptions);
        Assert.Equal(2, viewModel.RangeOptions.Count());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.ThisMonthOptionValue.ToString());
        Assert.Contains(viewModel.RangeOptions, x => x.Value == DateRangePickerViewModel.LastMonthOptionValue.ToString());
    }
}