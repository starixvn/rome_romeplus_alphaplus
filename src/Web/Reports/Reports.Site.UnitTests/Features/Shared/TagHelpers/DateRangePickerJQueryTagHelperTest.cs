using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class DateRangePickerJQueryTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_Base_Attributes()
    {
        // Arrange
        var model = new DateRangePickerJQueryViewModel(
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            language: "en"
        );

        var tagHelper = new DateRangePickerJQueryTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker-jquery",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
        Assert.Equal("form-group daterangepicker-container", output.Attributes["class"].Value);

        var content = output.Content.GetContent();
        Assert.Contains($"<label class=\"label-form-control\">{Resources.DateRange}</label>", content);
        Assert.Contains("data-mindate=\"01/01/2021\"", content);
        Assert.Contains("data-maxdate=\"12/31/2021\"", content);
        Assert.Contains("data-fromdate=\"06/01/2021\"", content);
        Assert.Contains("data-todate=\"06/30/2021\"", content);
        Assert.Contains("data-language=\"en\"", content);
    }

    [Fact]
    public void Process_Should_Not_Add_Shortcut_Attributes_When_ShowShortcuts_Is_False()
    {
        // Arrange
        var model = new DateRangePickerJQueryViewModel(
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            language: "en",
            showShortcuts: false,
            customShortcuts: new[] { DateRangePickerJQueryViewModel.TodayOptionValue }
        );

        var tagHelper = new DateRangePickerJQueryTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker-jquery",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain("data-today=", content);
        Assert.DoesNotContain("data-labeltoday=", content);
    }

    [Fact]
    public void Process_Should_Add_Today_Shortcut_When_Specified()
    {
        // Arrange
        var model = new DateRangePickerJQueryViewModel(
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            language: "en",
            showShortcuts: true,
            customShortcuts: new[] { DateRangePickerJQueryViewModel.TodayOptionValue }
        );

        var tagHelper = new DateRangePickerJQueryTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker-jquery",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"data-today=\"{model.Today}\"", content);
        Assert.Contains($"data-labeltoday=\"{Resources.Today}\"", content);
    }

    [Fact]
    public void Process_Should_Add_Multiple_Shortcuts_When_Specified()
    {
        // Arrange
        var model = new DateRangePickerJQueryViewModel(
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            language: "en",
            showShortcuts: true,
            customShortcuts: new[]
            {
                DateRangePickerJQueryViewModel.TodayOptionValue,
                DateRangePickerJQueryViewModel.YesterdayOptionValue,
                DateRangePickerJQueryViewModel.ThisWeekOptionValue,
                DateRangePickerJQueryViewModel.LastWeekOptionValue
            }
        );

        var tagHelper = new DateRangePickerJQueryTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker-jquery",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"data-labeltoday=\"{Resources.Today}\"", content);
        Assert.Contains($"data-labelyesterday=\"{Resources.Yesterday}\"", content);
        Assert.Contains($"data-labelthisweek=\"{Resources.ThisWeek}\"", content);
        Assert.Contains($"data-labellastweek=\"{Resources.LastWeek}\"", content);
    }

    [Fact]
    public void Process_Should_Add_All_Shortcut_Labels_When_Specified()
    {
        // Arrange
        var model = new DateRangePickerJQueryViewModel(
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            language: "en",
            showShortcuts: true,
            customShortcuts: new[]
            {
                DateRangePickerJQueryViewModel.TodayOptionValue,
                DateRangePickerJQueryViewModel.YesterdayOptionValue,
                DateRangePickerJQueryViewModel.ThisWeekOptionValue,
                DateRangePickerJQueryViewModel.LastWeekOptionValue,
                DateRangePickerJQueryViewModel.ThisMonthOptionValue,
                DateRangePickerJQueryViewModel.LastMonthOptionValue,
                DateRangePickerJQueryViewModel.SinceLastMonthOptionValue,
                DateRangePickerJQueryViewModel.Last3MonthsOptionValue
            }
        );

        var tagHelper = new DateRangePickerJQueryTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker-jquery",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"data-labeltoday=\"{Resources.Today}\"", content);
        Assert.Contains($"data-labelyesterday=\"{Resources.Yesterday}\"", content);
        Assert.Contains($"data-labelthisweek=\"{Resources.ThisWeek}\"", content);
        Assert.Contains($"data-labellastweek=\"{Resources.LastWeek}\"", content);
        Assert.Contains($"data-labelthismonth=\"{Resources.ThisMonth}\"", content);
        Assert.Contains($"data-labellastmonth=\"{Resources.LastMonth}\"", content);
        Assert.Contains($"data-labelsincelastmonth=\"{Resources.SinceLastMonth}\"", content);
        Assert.Contains($"data-labellast3months=\"{Resources.Last3Months}\"", content);
    }
}