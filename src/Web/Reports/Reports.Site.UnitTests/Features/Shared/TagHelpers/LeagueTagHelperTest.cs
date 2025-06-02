using AlphaPlus.Reports.Site.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class LeagueTagHelperTest
{
    [Fact]
    public void Process_Should_Set_Correct_TagName_And_TagMode()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string>(),
            IsUserSelectsAllLeagues: false,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_Required_Attributes_To_Output()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string>(),
            IsUserSelectsAllLeagues: false,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        Assert.Equal("form-group product-multiselect-container product-multiselect-long product-multiselect-two-col", output.Attributes["class"].Value);
        Assert.Equal("dropdown-league-container", output.Attributes["id"].Value);
    }

    [Fact]
    public void Process_Should_Hide_Dropdown_When_No_League_Selection()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string>(),
            IsUserSelectsAllLeagues: false,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("btn btn-default", content);
        Assert.DoesNotContain("btn btn-default hide", content);
        Assert.Contains("select-multiple hide", content);
    }

    [Fact]
    public void Process_Should_Show_Dropdown_When_User_Selects_All_Leagues()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string>(),
            IsUserSelectsAllLeagues: true,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("btn btn-default hide", content);
        Assert.Contains("select-multiple\"", content);
        Assert.DoesNotContain("select-multiple hide", content);
    }

    [Fact]
    public void Process_Should_Show_Dropdown_When_User_Has_Selected_Leagues()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string> { "1", "2" },
            IsUserSelectsAllLeagues: false,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => System.Threading.Tasks.Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("btn btn-default hide", content);
        Assert.Contains("select-multiple\"", content);
        Assert.DoesNotContain("select-multiple hide", content);
    }

    [Fact]
    public void Process_Should_Generate_Correct_Option_Elements()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>
            {
                new() { Value = "1", Text = "League 1", Selected = true },
                new() { Value = "2", Text = "League 2", Selected = false }
            },
            UserSelectedLeagueIds: new List<string> { "1" },
            IsUserSelectsAllLeagues: false,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => System.Threading.Tasks.Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<option value=\"1\" selected=\"selected\">League 1</option>", content);
        Assert.Contains("<option value=\"2\">League 2</option>", content);
    }

    [Fact]
    public void Process_Should_Include_Hidden_Inputs_With_Correct_Values()
    {
        // Arrange
        var model = new LeagueViewModel(
            Leagues: new List<SelectListItem>(),
            UserSelectedLeagueIds: new List<string> { "1", "2" },
            IsUserSelectsAllLeagues: true,
            Language: "en"
        );

        var helper = new LeagueTagHelper
        {
            Model = model
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");

        var output = new TagHelperOutput("league",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        helper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<input type=\"hidden\" name=\"UserSelectedLeagueIds\" id=\"league-ids\" value=\"1,2\" />", content);
        Assert.Contains("<input type=\"hidden\" name=\"IsUserSelectsAllLeagues\" id=\"IsUserSelectsAllLeagues\" value=\"True\" />", content);
    }
}