using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class DateRangePickerTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_Base_Attributes()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.SpecificDateOptionValue
        );

        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Null(output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);

        var content = output.Content.GetContent();
        Assert.Contains($"data-mindate=\"{model.MinDate}\"", content);
        Assert.Contains($"data-maxdate=\"{model.MaxDate}\"", content);
        Assert.Contains($"data-currentdate=\"{model.Today}\"", content);
        Assert.Contains($"value=\"{model.FromDate}\"", content);
        Assert.Contains($"value=\"{model.ToDate}\"", content);
        Assert.Contains($"<label for=\"from-date\" class=\"label-form-control\">{Resources.From}</label>", content);
        Assert.Contains($"<label for=\"to-date\" class=\"label-form-control\">{Resources.To}</label>", content);
    }

    [Fact]
    public void Process_Should_Use_CustomFromLabel_When_Provided()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.SpecificDateOptionValue,
            isRenderSubmitButton: false,
            optionValues: [DateRangePickerViewModel.TodayOptionValue]
        );

        var customFromLabel = "Custom From Label";
        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model,
            FromLabel = customFromLabel
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<label for=\"from-date\" class=\"label-form-control\">{customFromLabel}</label>", content);
    }

    [Fact]
    public void Process_Should_Render_Submit_Button_When_IsRenderSubmitButton_Is_True()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.SpecificDateOptionValue,
            isRenderSubmitButton: true,
            optionValues: [DateRangePickerViewModel.TodayOptionValue]
        );

        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"submit\" value=\"{Resources.Submit}\" class=\"btn-submit\" id=\"btn-submit\"/>", content);
    }

    [Fact]
    public void Process_Should_Not_Render_Submit_Button_When_IsRenderSubmitButton_Is_False()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.SpecificDateOptionValue,
            isRenderSubmitButton: false,
            optionValues: [DateRangePickerViewModel.TodayOptionValue]
        );

        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain($"<input type=\"submit\" value=\"{Resources.Submit}\" class=\"btn-submit\" id=\"btn-submit\"/>", content);
    }

    [Fact]
    public void Process_Should_Render_All_Range_Options()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.SpecificDateOptionValue,
            optionValues: [
                DateRangePickerViewModel.TodayOptionValue,
                DateRangePickerViewModel.YesterdayOptionValue,
                DateRangePickerViewModel.ThisWeekOptionValue,
                DateRangePickerViewModel.SpecificDateOptionValue
            ]
        );

        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"value=\"{DateRangePickerViewModel.TodayOptionValue}\"", content);
        Assert.Contains($"value=\"{DateRangePickerViewModel.YesterdayOptionValue}\"", content);
        Assert.Contains($"value=\"{DateRangePickerViewModel.ThisWeekOptionValue}\"", content);
        Assert.Contains($"value=\"{DateRangePickerViewModel.SpecificDateOptionValue}\"", content);
    }

    [Fact]
    public void Process_Should_Display_Selected_Range_Option()
    {
        // Arrange
        var model = new DateRangePickerViewModel(
            minDate: new DateTime(2021, 1, 1),
            maxDate: new DateTime(2021, 12, 31),
            fromDate: new DateTime(2021, 6, 1),
            toDate: new DateTime(2021, 6, 30),
            userSelectedDateRangeOption: DateRangePickerViewModel.TodayOptionValue
        );

        var tagHelper = new DateRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("daterange-picker",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"hidden\" name=\"UserSelectedDateRangeOption\" id=\"hidden-range-option\" value=\"{model.SelectedRangeOption.Value}\"/>", content);
        Assert.Contains($"<span id=\"range-label\">{model.SelectedRangeOption.Text}</span>", content);
    }
}
