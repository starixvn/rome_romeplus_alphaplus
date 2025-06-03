using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class MonthPickerTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Null(output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Generate_Container_With_Correct_Classes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"form-group monthpicker-container\">", content);
    }

    [Fact]
    public void Process_Should_Include_Dropdown_Month_Div_With_Data_Attributes()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div id=\"dropdown-month\"", content);
        Assert.Contains($"data-longmonths-inyear=\"{model.MonthNames}\"", content);
        Assert.Contains($"data-mindate=\"{model.MinDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-maxdate=\"{model.MaxDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-dateinmonth=\"{model.FromDate.ToShortDateWithSlash()}\"", content);
        Assert.Contains($"data-fromdate=\"{model.FromDate.Day}\"", content);
        Assert.Contains($"data-todate=\"{model.ToDate.Day}\"", content);
        Assert.Contains($"data-currentdate=\"{model.Today.ToShortDateWithSlash()}\"", content);
    }

    [Fact]
    public void Process_Should_Include_Hidden_Input_With_FromDate()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
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
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"hidden\" id=\"to-date\" name=\"ToDate\" value=\"{model.ToDate.ToShortDateWithSlash()}\" />", content);
    }

    [Fact]
    public void Process_Should_Generate_Correct_MonthPickerID_Container()
    {
        // Arrange
        var model = CreateViewModel();
        var helper = new MonthPickerTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("month-picker",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div id=\"monthPickerID\">", content);
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