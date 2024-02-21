using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class ChoiceTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-helper")]
    public ITicketHelper TicketHelper { get; set; }

    [HtmlAttributeName("ticket-data")]
    public List<ITicketData> TicketData { get; set; }

    [HtmlAttributeName("is-show-score-map")]
    public bool IsShowScoreMap { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"choice-block text-right sport-{Ticket.SportTypeId}");

        output.Content.SetHtmlContent(Builder.BuildChoice(Ticket, TicketHelper, TicketData, IsShowScoreMap));
    }
}