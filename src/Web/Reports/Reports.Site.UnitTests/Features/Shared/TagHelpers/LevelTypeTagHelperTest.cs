using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class LevelTypeTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = 0
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_Required_Class_To_Output()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = 0
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("form-group level-type-multiselect", output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Generate_Label_With_Correct_Text()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = 0
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<label class=\"label-form-control\">{Resources.Level}</label>", content);
    }

    [Fact]
    public void Process_Should_Generate_Select_With_Correct_Id()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = 0
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<select id=\"dropdown-level-type\">", content);
    }

    [Theory]
    [InlineData(0, true, false, false)]
    [InlineData(1, false, true, false)]
    [InlineData(2, false, false, true)]
    public void Process_Should_Mark_Correct_Option_As_Selected(byte selectedLevel, bool isAllSelected, bool isLevel1Selected, bool isLevel2Selected)
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = selectedLevel
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Equal(isAllSelected, content.Contains("<option value=\"0\" selected>"));
        Assert.Equal(isLevel1Selected, content.Contains("<option value=\"1\" selected>"));
        Assert.Equal(isLevel2Selected, content.Contains("<option value=\"2\" selected>"));
    }

    [Fact]
    public void Process_Should_Use_MasterAgent_Text_For_SuperLevel()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = UserLevel.Super,
            SelectedLevel = 1
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<option value=\"1\" selected>{CommonResx.MasterAgent}</option>", content);
    }

    [Fact]
    public void Process_Should_Use_Agent_Text_For_Non_SuperLevel()
    {
        // Arrange
        var helper = new LevelTypeTagHelper
        {
            CustLevel = UserLevel.Master,
            SelectedLevel = 1
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<option value=\"1\" selected>{Resources.Agent}</option>", content);
    }

    [Fact]
    public void Process_Should_Include_Hidden_Input_With_Correct_Value()
    {
        // Arrange
        var selectedLevel = (byte)1;
        var helper = new LevelTypeTagHelper
        {
            CustLevel = new UserLevel(),
            SelectedLevel = selectedLevel
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("level-type",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"<input type=\"hidden\" name=\"LevelType\" id=\"level-type\" value=\"{selectedLevel}\" />", content);
    }
}