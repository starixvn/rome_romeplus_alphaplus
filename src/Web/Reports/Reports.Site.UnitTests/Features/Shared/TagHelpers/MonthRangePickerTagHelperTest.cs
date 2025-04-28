using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class MonthRangePickerTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Null(output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Generate_From_Date_Container_With_Correct_Classes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"form-group\">", content);
        Assert.Contains("<div class=\"control month-range-picker-element\">", content);
        Assert.Contains($"<label class=\"label-form-control\">{Resources.From}</label>", content);
    }

    [Fact]
    public void Process_Should_Generate_To_Date_Container_With_Correct_Classes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<label class=\"label-form-control\">{Resources.To}</label>", content);
    }

    [Fact]
    public void Process_Should_Include_From_Monthpicker_Div_With_Data_Attributes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div id=\"from-monthpicker\"", content);
        Assert.Contains($"data-longmonths-inyear=\"{model.MonthNames}\"", content);
        Assert.Contains($"data-mindate=\"{model.MinDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-maxdate=\"{model.MaxDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-dateinmonth=\"{model.FromDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-currentdate=\"{model.ToDate.ToShortDateWithSlash()}\"", content);
    }

    [Fact]
    public void Process_Should_Include_To_Monthpicker_Div_With_Data_Attributes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div id=\"to-monthpicker\"", content);
        Assert.Contains($"data-longmonths-inyear=\"{model.MonthNames}\"", content);
        Assert.Contains($"data-mindate=\"{model.MinDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-maxdate=\"{model.MaxDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-dateinmonth=\"{model.ToDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-currentdate=\"{model.ToDate.ToShortDateWithSlash()}\"", content);
    }

    [Fact]
    public void Process_Should_Include_Hidden_Input_With_FromDate()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"hidden\" id=\"from-date\" name=\"FromDate\" value=\"{model.FromDate.ToShortDateWithSlash()}\" />", content);
    }

    [Fact]
    public void Process_Should_Include_Hidden_Input_With_ToDate()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthRangePickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-range-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"hidden\" id=\"to-date\" name=\"ToDate\" value=\"{model.ToDate.ToShortDateWithSlash()}\" />", content);
    }

    private MonthPickerViewModel CreateViewModel()
    {
        return new MonthPickerViewModel(
            MinDate: new DateTime(2022, 1, 1),
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31)
        );
    }
}