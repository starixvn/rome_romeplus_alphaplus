using System.Text.Encodings.Web;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class StakeTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("stake-block bl_underdog text-right", HtmlEncoder.Default);

        output.Content.SetContent(Builder.BuildStake(Ticket, TicketHelper));
    }
}