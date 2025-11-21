using System.Text;
using System.Text.Encodings.Web;
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
        var encodedOriginal = HtmlEncoder.Default.Encode(OriginalOdds ?? string.Empty);
        var encodedOdds = HtmlEncoder.Default.Encode(Odds ?? string.Empty);
        var sb = new StringBuilder();
        sb
            .Append($"<span class=\"underdog strikethrough\">{encodedOriginal}</span><br/>")
            .Append("<span class=\"oddstype\">")
            .Append($"<span class=\"underdog\">{encodedOdds}</span>")
            .Append("</span>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}