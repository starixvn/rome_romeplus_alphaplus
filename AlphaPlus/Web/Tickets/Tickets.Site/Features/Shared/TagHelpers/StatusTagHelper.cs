using System.Text;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class StatusTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public TicketDto Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    [HtmlAttributeName("ticket-data")]
    public List<ITicketData> TicketData { get; set; }

    [HtmlAttributeName("is-show-vn-ip")]
    public bool IsShowVNIP { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"status-block text-center sport-{Ticket.SportTypeId}");

        var status = new StringBuilder();
        status.Append(Builder.BuildStatus(Ticket, TicketHelper, TicketData, IsShowVNIP));

        if (!string.IsNullOrWhiteSpace(Ticket.Country))
        {
            status.Append($"<div class=\"country\">{Ticket.Country}</div>");
        }

        output.Content.SetHtmlContent(status.ToString());
    }
}