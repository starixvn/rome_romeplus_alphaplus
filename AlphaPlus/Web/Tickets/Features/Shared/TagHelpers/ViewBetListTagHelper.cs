using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("view-betlist")]
public class ViewBetListTagHelper : TagHelper
{
    [HtmlAttributeName("matchid")]
    public int MatchId { get; set; }

    [HtmlAttributeName("bettype")]
    public int BetType { get; set; }

    [HtmlAttributeName("value")]
    public decimal? Value { get; set; }

    [HtmlAttributeName("redpositive")]
    public bool? RedPositive { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("fcnum", HtmlEncoder.Default);
        output.Attributes.Add("href", $"../site-betlists/BetListByBetTypeAndMatch/Index?matchid={MatchId}bettype={BetType}");

        if (Value.HasValue)
        {
            var sb = new StringBuilder();
            var number = Value.Value;
            var formatedNumber = number.ToString("N0");

            if (RedPositive.HasValue
            && ((RedPositive == true && number > 0) || (RedPositive == false && number < 0)))
            {
                sb.Append($"<span class=\"positive\">{formatedNumber}</span>");
            }
            else
            {
                sb.Append(formatedNumber);
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
        else
        {
            //output.Content.SetHtmlContent();
        }
    }
}