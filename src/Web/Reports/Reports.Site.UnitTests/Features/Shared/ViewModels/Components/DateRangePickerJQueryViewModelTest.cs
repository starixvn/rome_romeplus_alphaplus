using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests.ViewModels;

[Trait("Category", "Shared")]
public class DateRangePickerJQueryViewModelTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var minDate = new DateTime(2022, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var language = "en-US";

        // Act
        var viewModel = new DateRangePickerJQueryViewModel(
            fromDate, toDate, minDate, maxDate, language);

        // Assert
        Assert.Equal(fromDate.ToShortDateWithSlash(), viewModel.FromDate);
        Assert.Equal(toDate.ToShortDateWithSlash(), viewModel.ToDate);
        Assert.Equal(minDate.ToShortDateWithSlash(), viewModel.MinDate);
        Assert.Equal(maxDate.ToShortDateWithSlash(), viewModel.MaxDate);
        Assert.Equal(language, viewModel.Language);
        Assert.True(viewModel.ShowShortcuts);
        Assert.NotNull(viewModel.CustomShortcuts);
        Assert.Equal(8, viewModel.CustomShortcuts.Length);
        Assert.Equal(SystemTime.Today.ToShortDateWithSlash(), viewModel.Today);
    }

    [Fact]
    public void Constructor_With_ShowShortcuts_False_Should_Set_CustomShortcuts_To_Null()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var minDate = new DateTime(2022, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var language = "en-US";
        var showShortcuts = false;

        // Act
        var viewModel = new DateRangePickerJQueryViewModel(
            fromDate, toDate, minDate, maxDate, language, showShortcuts);

        // Assert
        Assert.False(viewModel.ShowShortcuts);
        Assert.Null(viewModel.CustomShortcuts);
    }

    [Fact]
    public void Constructor_With_CustomShortcuts_Should_Use_Provided_Values()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var minDate = new DateTime(2022, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var language = "en-US";
        var customShortcuts = new int[] {
            DateRangePickerJQueryViewModel.TodayOptionValue,
            DateRangePickerJQueryViewModel.YesterdayOptionValue
        };

        // Act
        var viewModel = new DateRangePickerJQueryViewModel(
            fromDate, toDate, minDate, maxDate, language, true, customShortcuts);

        // Assert
        Assert.True(viewModel.ShowShortcuts);
        Assert.Equal(customShortcuts, viewModel.CustomShortcuts);
        Assert.Equal(2, viewModel.CustomShortcuts.Length);
    }

    [Fact]
    public void ConstantValues_Should_Have_Expected_Values()
    {
        // Assert
        Assert.Equal(0, DateRangePickerJQueryViewModel.TodayOptionValue);
        Assert.Equal(1, DateRangePickerJQueryViewModel.YesterdayOptionValue);
        Assert.Equal(2, DateRangePickerJQueryViewModel.ThisWeekOptionValue);
        Assert.Equal(3, DateRangePickerJQueryViewModel.LastWeekOptionValue);
        Assert.Equal(4, DateRangePickerJQueryViewModel.ThisMonthOptionValue);
        Assert.Equal(5, DateRangePickerJQueryViewModel.LastMonthOptionValue);
        Assert.Equal(6, DateRangePickerJQueryViewModel.SinceLastMonthOptionValue);
        Assert.Equal(7, DateRangePickerJQueryViewModel.Last3MonthsOptionValue);
    }

    [Fact]
    public void Today_Property_Should_Return_SystemTime_Today()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var minDate = new DateTime(2022, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var language = "en-US";

        // Act
        var viewModel = new DateRangePickerJQueryViewModel(
            fromDate, toDate, minDate, maxDate, language);

        // Assert
        Assert.Equal(SystemTime.Today.ToShortDateWithSlash(), viewModel.Today);
    }
}