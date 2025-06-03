using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class UsernameAutoSuggestionTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_FormGroup_Classes()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.True(output.Attributes.ContainsName("class"));
        Assert.Equal("form-group form-group-username", output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Generate_Label_With_Username_Text()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);
        var content = output.Content.GetContent();

        // Assert
        Assert.Contains($"<label class=\"label-form-control\">{Resources.Username}</label>", content);
    }

    [Fact]
    public void Process_Should_Generate_Input_With_Placeholder()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);
        var content = output.Content.GetContent();

        // Assert
        Assert.Contains($"placeholder=\"{Resources.UsernameOrFirstLastName}\"", content);
    }

    [Fact]
    public void Process_Should_Set_Empty_Value_By_Default()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);
        var content = output.Content.GetContent();

        // Assert
        Assert.Contains("value=\"\"", content);
    }

    [Fact]
    public void Process_Should_Use_Provided_Value()
    {
        // Arrange
        var tagHelper = new UsernameAutoSuggestionTagHelper
        {
            Value = "testUser"
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "testId");
        var output = new TagHelperOutput(
            "username-autosuggestion",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);
        var content = output.Content.GetContent();

        // Assert
        Assert.Contains("value=\"testUser\"", content);
    }
}