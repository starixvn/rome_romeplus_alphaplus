using System.Text.Encodings.Web;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class TransTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("trans-block text-center nonbreak", HtmlEncoder.Default);

        output.Content.SetContent($"<div class=\"time\">{Builder.BuildTrans(Ticket)}</div>");
    }
}