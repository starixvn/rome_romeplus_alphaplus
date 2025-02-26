using System.Text;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders;

public class StatusTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    [HtmlAttributeName("ticket-data")]
    public List<ITicketData> TicketData { get; set; }

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
        status.Append(Builder.BuildStatus(Ticket, TicketHelper, TicketData, IsShowVNIP));

        if (!string.IsNullOrWhiteSpace(Country))
        {
            status.Append($"<div class=\"country\">{Country}</div>");
        }

        output.Content.SetHtmlContent(status.ToString());
    }
}