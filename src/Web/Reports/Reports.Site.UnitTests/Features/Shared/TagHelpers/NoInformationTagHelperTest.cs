using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class NoInformationTagHelperTest
{
    [Fact]
    public void Process_Should_Set_TagName_To_Tbody()
    {
        // Arrange
        var tagHelper = new NoInformationTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("no-information",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("tbody", output.TagName);
    }

    [Fact]
    public void Process_Should_Set_TagMode_To_StartTagAndEndTag()
    {
        // Arrange
        var tagHelper = new NoInformationTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("no-information",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Process_Should_Set_Content_With_Correct_Colspan_Value(int colspan)
    {
        // Arrange
        var tagHelper = new NoInformationTagHelper { Colspan = colspan };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("no-information",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"colspan=\"{colspan}\"", content);
    }

    [Fact]
    public void Process_Should_Include_NoInformationIsAvailable_Text()
    {
        // Arrange
        var tagHelper = new NoInformationTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("no-information",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains(Resources.NoInformationIsAvailable, content);
    }

    [Fact]
    public void Process_Should_Add_Text_Center_And_First_Row_Classes()
    {
        // Arrange
        var tagHelper = new NoInformationTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("no-information",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("class=\"text-center first-row\"", content);
    }
}