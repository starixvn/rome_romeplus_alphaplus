using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using static AlphaPlus.Shared.AppResources.Report.Resources;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

[Trait("Category", "Shared")]
public class MonthPickerViewModelTest
{
    [Fact]
    public void Constructor_WithDefaultParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var fromDate = new DateTime(2023, 3, 15);
        var toDate = new DateTime(2023, 6, 30);

        // Act
        var viewModel = new MonthPickerViewModel(minDate, fromDate, toDate);

        // Assert
        Assert.Equal(minDate, viewModel.MinDate);
        Assert.Equal(fromDate, viewModel.FromDate);
        Assert.Equal(toDate, viewModel.ToDate);
        Assert.False(viewModel.IsViewNextYear);
        Assert.Equal(SystemTime.Today, viewModel.Today);
        Assert.Equal(SystemTime.Today, viewModel.MaxDate);
    }

    [Fact]
    public void Constructor_WithIsViewNextYearTrue_SetsMaxDateToNextYearEnd()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var fromDate = new DateTime(2023, 3, 15);
        var toDate = new DateTime(2023, 6, 30);
        var isViewNextYear = true;

        // Act
        var viewModel = new MonthPickerViewModel(minDate, fromDate, toDate, isViewNextYear);

        // Assert
        Assert.Equal(SystemTime.Today.AddYears(1).LastDayOfYear(), viewModel.MaxDate);
    }

    [Fact]
    public void MonthNames_ReturnsCommaSeparatedListOfMonthNames()
    {
        // Arrange
        var viewModel = new MonthPickerViewModel(DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        var expectedMonthNames = string.Join(',', new[]
        {
            January, February, March, April, May, June,
            July, August, September, October, November, December
        });

        // Act & Assert
        Assert.Equal(expectedMonthNames, viewModel.MonthNames);
    }

    [Fact]
    public void MonthPickerViewModel_IsRecordType_SupportsValueEquality()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var fromDate = new DateTime(2023, 3, 15);
        var toDate = new DateTime(2023, 6, 30);
        var isViewNextYear = true;

        var viewModel1 = new MonthPickerViewModel(minDate, fromDate, toDate, isViewNextYear);
        var viewModel2 = new MonthPickerViewModel(minDate, fromDate, toDate, isViewNextYear);

        // Act & Assert
        Assert.Equal(viewModel1, viewModel2);
        Assert.True(viewModel1 == viewModel2);
    }

    [Fact]
    public void MonthPickerViewModel_IsRecordType_DetectsDifferentValues()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var fromDate = new DateTime(2023, 3, 15);
        var toDate = new DateTime(2023, 6, 30);

        var viewModel1 = new MonthPickerViewModel(minDate, fromDate, toDate, true);
        var viewModel2 = new MonthPickerViewModel(minDate, fromDate, toDate, false);

        // Act & Assert
        Assert.NotEqual(viewModel1, viewModel2);
        Assert.False(viewModel1 == viewModel2);
    }
}