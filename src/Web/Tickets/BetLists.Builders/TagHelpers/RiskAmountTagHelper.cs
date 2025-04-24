using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders.TagHelpers;

[HtmlTargetElement("risk-amount")]
public class RiskAmountTagHelper : TagHelper
{
    [HtmlAttributeName("member-win")]
    public string MemberWin { get; set; }

    [HtmlAttributeName("sma-risk-amount")]
    public string SMARiskAmount { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();
        sb.Append($"<td class=\"member-risk-block bl_underdog text-center\">{MemberWin}</td>");
        sb.Append($"<td class=\"member-risk-block bl_underdog text-center\">{SMARiskAmount}</td>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}