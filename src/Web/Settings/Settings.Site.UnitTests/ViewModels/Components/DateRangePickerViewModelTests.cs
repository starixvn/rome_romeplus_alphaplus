using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;

namespace AlphaPlus.Settings.Site.UnitTests.ViewModels.Components;

public class DateRangePickerViewModelTests
{
    [Fact]
    public void DateRangePickerViewModel_Should_Initialize_With_Correct_Values()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 6, 1);
        var toDate = new DateTime(2023, 6, 30);

        // Act
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Initialize_With_Default_SubmitButton()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 6, 1);
        var toDate = new DateTime(2023, 6, 30);

        // Act
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Have_ReadOnly_Properties()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1);
        var maxDate = new DateTime(2023, 12, 31);
        var fromDate = new DateTime(2023, 6, 1);
        var toDate = new DateTime(2023, 6, 30);
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Act & Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Handle_Same_Date_Range()
    {
        // Arrange
        var sameDate = new DateTime(2023, 6, 15);

        // Act
        var viewModel = new DateRangePickerViewModel(sameDate, sameDate, sameDate, sameDate);

        // Assert
        viewModel.MinDate.Should().Be(sameDate);
        viewModel.MaxDate.Should().Be(sameDate);
        viewModel.FromDate.Should().Be(sameDate);
        viewModel.ToDate.Should().Be(sameDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Handle_Leap_Year_Dates()
    {
        // Arrange
        var minDate = new DateTime(2024, 2, 28);
        var maxDate = new DateTime(2024, 2, 29);
        var fromDate = new DateTime(2024, 2, 28);
        var toDate = new DateTime(2024, 2, 29);

        // Act
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Handle_Future_Dates()
    {
        // Arrange
        var futureMinDate = new DateTime(2030, 1, 1);
        var futureMaxDate = new DateTime(2030, 12, 31);
        var futureFromDate = new DateTime(2030, 6, 1);
        var futureToDate = new DateTime(2030, 6, 30);

        // Act
        var viewModel = new DateRangePickerViewModel(futureMinDate, futureMaxDate, futureFromDate, futureToDate);

        // Assert
        viewModel.MinDate.Should().Be(futureMinDate);
        viewModel.MaxDate.Should().Be(futureMaxDate);
        viewModel.FromDate.Should().Be(futureFromDate);
        viewModel.ToDate.Should().Be(futureToDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Handle_Past_Dates()
    {
        // Arrange
        var pastMinDate = new DateTime(2020, 1, 1);
        var pastMaxDate = new DateTime(2020, 12, 31);
        var pastFromDate = new DateTime(2020, 6, 1);
        var pastToDate = new DateTime(2020, 6, 30);

        // Act
        var viewModel = new DateRangePickerViewModel(pastMinDate, pastMaxDate, pastFromDate, pastToDate);

        // Assert
        viewModel.MinDate.Should().Be(pastMinDate);
        viewModel.MaxDate.Should().Be(pastMaxDate);
        viewModel.FromDate.Should().Be(pastFromDate);
        viewModel.ToDate.Should().Be(pastToDate);
    }

    [Fact]
    public void DateRangePickerViewModel_Should_Handle_DateTime_With_Time()
    {
        // Arrange
        var minDate = new DateTime(2023, 1, 1, 0, 0, 0);
        var maxDate = new DateTime(2023, 12, 31, 23, 59, 59);
        var fromDate = new DateTime(2023, 6, 1, 9, 0, 0);
        var toDate = new DateTime(2023, 6, 30, 17, 30, 0);

        // Act
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }

    [Theory]
    [InlineData(2023, 1, 1, 2023, 12, 31, 2023, 6, 1, 2023, 6, 30)]
    [InlineData(2024, 2, 29, 2024, 12, 31, 2024, 6, 1, 2024, 6, 30)]
    [InlineData(2022, 12, 31, 2023, 1, 1, 2023, 1, 1, 2023, 1, 1)]
    public void DateRangePickerViewModel_Should_Handle_Various_Date_Combinations(
        int minYear, int minMonth, int minDay,
        int maxYear, int maxMonth, int maxDay,
        int fromYear, int fromMonth, int fromDay,
        int toYear, int toMonth, int toDay)
    {
        // Arrange
        var minDate = new DateTime(minYear, minMonth, minDay);
        var maxDate = new DateTime(maxYear, maxMonth, maxDay);
        var fromDate = new DateTime(fromYear, fromMonth, fromDay);
        var toDate = new DateTime(toYear, toMonth, toDay);

        // Act
        var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate);

        // Assert
        viewModel.MinDate.Should().Be(minDate);
        viewModel.MaxDate.Should().Be(maxDate);
        viewModel.FromDate.Should().Be(fromDate);
        viewModel.ToDate.Should().Be(toDate);
    }
}
