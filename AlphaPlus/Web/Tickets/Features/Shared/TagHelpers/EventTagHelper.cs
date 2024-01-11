using System.Text;
using System.Text.Encodings.Web;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class EventTagHelper : TagHelper
{
    [HtmlAttributeName("live")]
    public bool LiveIndicator { get; set; }

    [HtmlAttributeName("homescore")]
    public int LiveHomeScore { get; set; }

    [HtmlAttributeName("awayscore")]
    public int LiveAwayScore { get; set; }

    [HtmlAttributeName("time")]
    public string ShowTime { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("col-time", HtmlEncoder.Default);

        if (LiveIndicator)
        {
            var sb = new StringBuilder();
            sb
                .Append($"<div>{LiveHomeScore} - {LiveAwayScore}</div>")
                .Append($"<span class=\"live\">{Resources.Live}!</span>");
            output.Content.SetHtmlContent(sb.ToString());
        }
        else
        {
            output.Content.SetHtmlContent(ShowTime);
        }
    }
}