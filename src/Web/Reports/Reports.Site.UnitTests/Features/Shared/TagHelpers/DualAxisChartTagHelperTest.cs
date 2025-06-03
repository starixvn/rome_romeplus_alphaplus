using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class DualAxisChartTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var model = new DualAxisChartViewModel(
            Title: "Test Title",
            Labels: "Jan,Feb,Mar",
            FirstData: "10,20,30",
            SecondData: "5,15,25",
            FirstDataLabel: "First Axis",
            SecondDataLabel: "Second Axis",
            CurrencyName: "USD"
        );

        var helper = new DualAxisChartTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("dual-axis-chart",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("canvas", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_Required_Attributes_From_Model()
    {
        // Arrange
        var model = new DualAxisChartViewModel(
            Title: "Test Title",
            Labels: "Jan,Feb,Mar",
            FirstData: "10,20,30",
            SecondData: "5,15,25",
            FirstDataLabel: "First Axis",
            SecondDataLabel: "Second Axis",
            CurrencyName: "USD"
        );

        var helper = new DualAxisChartTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("dual-axis-chart",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("winloss-chart", output.Attributes["id"].Value);
        Assert.Equal("Test Title", output.Attributes["data-title"].Value);
        Assert.Equal("Jan,Feb,Mar", output.Attributes["data-labels"].Value);
        Assert.Equal("10,20,30", output.Attributes["data-y-axis-1"].Value);
        Assert.Equal("5,15,25", output.Attributes["data-y-axis-2"].Value);
        Assert.Equal("First Axis", output.Attributes["data-y-axis-1-label"].Value);
        Assert.Equal("Second Axis", output.Attributes["data-y-axis-2-label"].Value);
        Assert.Equal("USD", output.Attributes["data-currency-name"].Value);
    }

    [Fact]
    public void Process_Should_Handle_Null_Model_Values()
    {
        // Arrange
        var model = new DualAxisChartViewModel(
            Title: null,
            Labels: null,
            FirstData: null,
            SecondData: null,
            FirstDataLabel: null,
            SecondDataLabel: null,
            CurrencyName: null
        );

        var helper = new DualAxisChartTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("dual-axis-chart",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("winloss-chart", output.Attributes["id"].Value);
        Assert.Null(output.Attributes["data-title"].Value);
        Assert.Null(output.Attributes["data-labels"].Value);
        Assert.Null(output.Attributes["data-y-axis-1"].Value);
        Assert.Null(output.Attributes["data-y-axis-2"].Value);
        Assert.Null(output.Attributes["data-y-axis-1-label"].Value);
        Assert.Null(output.Attributes["data-y-axis-2-label"].Value);
        Assert.Null(output.Attributes["data-currency-name"].Value);
    }
}