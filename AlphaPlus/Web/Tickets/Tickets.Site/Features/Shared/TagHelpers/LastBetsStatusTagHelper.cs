using System.Text;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("lastbets-status")]
public class LastBetsStatusTagHelper : TagHelper
{
    [HtmlAttributeName("ticket")]
    public TicketDto Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    [HtmlAttributeName("is-show-vn-ip")]
    public bool IsShowVNIP { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"status-block text-center sport-{Ticket.SportTypeId}");

        var status = new StringBuilder();
        if (!IsShowVNIP && TicketHelper.IsVnIp(Ticket.IP))
        {
            status.Append("<div class=\"ip\"><div class=\"iplink\">--</div></div>");
        }
        else
        {
            status.Append($"<div class=\"ip\"><div class=\"iplink\" onclick=\"OpenIPInfo('{Ticket.IP}');\">{Ticket.IP}</div></div>");
            if (!string.IsNullOrWhiteSpace(Ticket.Country))
            {
                status.Append($"<div class=\"country\">{Ticket.Country}</div>");
            }
        }

        output.Content.SetHtmlContent(status.ToString());
    }
}