using AlphaPlus.Tickets.Site.Features.BetLists.Builders.HtmlBuilders;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("position-taking")]
public class PositionTakingTagHelper : TagHelper
{
    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-data")]
    public List<ITicketData> TicketData { get; set; }

    [HtmlAttributeName("user-level")]
    public Core.Models.UserLevel CustLevel { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "position-taking-block text-right");

        output.Content.SetHtmlContent(BetListHtmlBuilderExt.BuildPtComm(Ticket, TicketData, CustLevel));
    }
}