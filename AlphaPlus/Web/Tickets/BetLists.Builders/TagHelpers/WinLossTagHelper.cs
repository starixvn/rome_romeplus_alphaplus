using System.Text;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders;

[HtmlTargetElement("winloss")]
public class WinLossTagHelper : TagHelper
{
    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new WinLossBuilder(Ticket);

        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "win-loss-block text-right nonbreak");

        var winloss = new StringBuilder();
        winloss.Append($"<div><span class=\"bl_underdog\">{builder.WinLoss}</span></div>");

        if (builder.CommVisibility)
        {
            winloss.Append($"<div><span class=\"bl_underdog\">{builder.Comm}</span></div>");
        }

        output.Content.SetHtmlContent(winloss.ToString());
    }
}