using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders;

[HtmlTargetElement("bet-builder-plus-odds")]
public class BetBuilderPlusOddsTagHelper : TagHelper
{
    [HtmlAttributeName("original-odds")]
    public string OriginalOdds { get; set; }

    [HtmlAttributeName("odds")]
    public string Odds { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        var sb = new StringBuilder();
        sb
            .Append($"<span class=\"underdog strikethrough\">{OriginalOdds}</span><br/>")
            .Append("<span class=\"oddstype\">")
            .Append($"<span class=\"underdog\">{Odds}</span>")
            .Append("</span>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}