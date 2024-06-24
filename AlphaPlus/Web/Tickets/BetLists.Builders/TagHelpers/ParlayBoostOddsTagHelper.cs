using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.BetLists.Builders;

[HtmlTargetElement("parlay-boost-odds")]
public class ParlayBoostOddsTagHelper : TagHelper
{
    [HtmlAttributeName("original-odds")]
    public string OriginalOdds { get; set; }

    [HtmlAttributeName("boost-odds")]
    public string BoostOdds { get; set; }

    [HtmlAttributeName("boost-rate")]
    public string BoostRate { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        var sb = new StringBuilder();
        sb
            .Append($"<span class=\"underdog strikethrough\">{OriginalOdds}</span><br/>")
            .Append("<span class=\"oddstype\">")
            .Append($"<span class=\"underdog\">{BoostOdds}</span><br/>")
            .Append($"<span class=\"boostrate\"><span class=\"icon-boost\"></span>${BoostRate}</span>")
            .Append("</span>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}