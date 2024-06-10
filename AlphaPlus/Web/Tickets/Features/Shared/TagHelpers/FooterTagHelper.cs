using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.BetLists.AppResources;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class FooterTagHelper : TagHelper
{
    [HtmlAttributeName("subtotal-colspan")]
    public int SubTotalColSpan { get; set; }

    [HtmlAttributeName("status-colspan")]
    public int StatusColSpan { get; set; }

    [HtmlAttributeName("subtotal-winloss")]
    public string SubTotalWinLoss { get; set; }

    [HtmlAttributeName("subtotal-commission")]
    public string SubTotalCommission { get; set; }

    [HtmlAttributeName("total")]
    public string Total { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tr";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("bl_footertotal", HtmlEncoder.Default);

        var footer = new StringBuilder();
        footer
            .Append($"<td class=\"text-right\" colspan=\"{SubTotalColSpan}\">")
            .Append($"<div>{Resources.Subtotal_wl}:</div>")
            .Append($"<div>{Resources.Subtotal_com}:</div>")
            .Append($"<div>{Resources.total}:</div>")
            .Append("</td>")
            .Append("<td class=\"text-right\">")
            .Append($"<div>{SubTotalWinLoss}</div>")
            .Append($"<div>{SubTotalCommission}</div>")
            .Append($"<div>{Total}</div>")
            .Append("</td>")
            .Append($"<td colspan=\"{StatusColSpan}\"></td>");

        output.Content.SetHtmlContent(footer.ToString());
    }
}