using System.Text.Encodings.Web;
using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("view-betlist-icon")]
public class ViewBetListIconTagHelper : TagHelper
{
    [HtmlAttributeName("matchid")]
    public int MatchId { get; set; }

    [HtmlAttributeName("bettype")]
    public int BetType { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("icon-fc", HtmlEncoder.Default);
        output.Attributes.Add("href", MicroSites.TicketUrl($"~/BetListByBetTypeAndMatch/Index?matchid={MatchId}&bettype={BetType}"));
        output.Content.SetHtmlContent("&nbsp;");
    }
}