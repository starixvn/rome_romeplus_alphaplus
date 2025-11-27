using System.Text;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders;

[HtmlTargetElement("lastbets-status")]
public class LastBetsStatusTagHelper : TagHelper
{
    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    [HtmlAttributeName("is-show-vn-ip")]
    public bool IsShowVNIP { get; set; }

    [HtmlAttributeName("country")]
    public string Country { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"status-block text-center sport-{Ticket.SportTypeId}");

        var status = new StringBuilder();
        if (IsShowVNIP || !TicketHelper.IsVnIp(Ticket.IP))
        {
            status.Append($"<div class=\"ip\"><div class=\"iplink\" onclick=\"OpenIPInfo('{Ticket.IP}');\">{Ticket.IP}</div></div>");
            if (!string.IsNullOrWhiteSpace(Country))
            {
                status.Append($"<div class=\"country\">{Country}</div>");
            }
        }

        output.Content.SetHtmlContent(status.ToString());
    }
}