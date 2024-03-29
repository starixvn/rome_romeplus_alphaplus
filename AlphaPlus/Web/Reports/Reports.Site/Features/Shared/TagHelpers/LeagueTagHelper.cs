using System.Text;
using System.Text.Encodings.Web;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.MatchWinLossDetail;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

public class LeagueTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public LeagueViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var isShowDropdown = Model.IsUserSelectsAllLeagues || Model.UserSelectedLeagueIds?.Any() == true;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("form-group", HtmlEncoder.Default);
        output.Attributes.Add("id", "dropdown-league-container");

        var sb = new StringBuilder();
        sb
            .Append("<div class=\"btn-group btn-loadleague-container\">")
            .Append($"<button id=\"btn-loadleague\" type=\"button\" class=\"multiselect dropdown-toggle btn btn-default{(isShowDropdown ? " hide" : string.Empty)}\" data-toggle=\"dropdown\">")
            .Append($"<span class=\"multiselect-selected-text\">{Resources.AllLeagues}</span>")
            .Append("<b class=\"caret\"></b></button></div>");
        sb
            .Append("<select id=\"dropdown-league\" ")
            .Append($"data-hasdata=\"{Model.Leagues.Any().ToString().ToLowerInvariant()}\" ")
            .Append($"data-label-selectleagues=\"{Resources.SelectLeague}\" ")
            .Append($"data-label-selected=\"{Resources.Selected}\" ")
            .Append($"data-label-allleagues=\"{Resources.AllLeagues}\" ")
            .Append($"data-label-nodata=\"{Resources.ThereIsNoLeague}\" ")
            .Append($"data-language=\"{Model.Language}\" multiple=\"multiple\" class=\"select-multiple{(isShowDropdown ? string.Empty : " hide")}\">");
        foreach (var league in Model.Leagues)
        {
            sb.Append($"<option value=\"{league.Value}\"{(league.Selected ? " selected=\"selected\"" : string.Empty)}>{league.Text}</option>");
        }

        sb
            .Append("</select>")
            .Append($"<input type=\"hidden\" name=\"UserSelectedLeagueIds\" id=\"league-ids\" value=\"{Model.UserSelectedLeagueIds.JoinWithComma()}\" />")
            .Append($"<input type=\"hidden\" name=\"IsUserSelectsAllLeagues\" id=\"IsUserSelectsAllLeagues\" value=\"{Model.IsUserSelectsAllLeagues}\" />");

        output.Content.SetHtmlContent(sb.ToString());
    }
}