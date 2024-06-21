using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class OddsTagHelper : TagHelper
{
    [HtmlAttributeName("builder")]
    public IBetListHTMLBuilder Builder { get; set; }

    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    [HtmlAttributeName("ticket-data")]
    public List<ITicketData> TicketData { get; set; }

    [HtmlAttributeName("get-cache-property-by-id")]
    public GetCachePropertyById GetOddsTypeNameFunc { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "odds-block bl_underdog nonbreak text-right");

        output.Content.SetHtmlContent(Builder.BuildOdds(Ticket, TicketData, GetOddsTypeNameFunc));
    }
}