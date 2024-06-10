using System.Text;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Blocks;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("winloss")]
public class WinLossTagHelper : TagHelper
{
    [HtmlAttributeName("ticket")]
    public ITicket Ticket { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var model = new WinLostBlockViewModel(Ticket);

        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "win-loss-block text-right nonbreak");

        var winloss = new StringBuilder();
        winloss.Append($"<div><span class=\"bl_underdog\">{model.WinLost}</span></div>");

        if (model.CommVisibility)
        {
            winloss.Append($"<div><span class=\"bl_underdog\">{model.Comm}</span></div>");
        }

        output.Content.SetHtmlContent(winloss.ToString());
    }
}