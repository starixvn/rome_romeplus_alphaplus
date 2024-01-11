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

    [HtmlAttributeName("islive")]
    public byte? IsLive { get; set; }

    [HtmlAttributeName("betteam")]
    public string BetTeam { get; set; }

    [HtmlAttributeName("value")]
    public decimal? Value { get; set; }

    [HtmlAttributeName("redpositive")]
    public bool? RedPositive { get; set; }

    [HtmlAttributeName("text")]
    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;
        var link = $"../site-betlists/BetListByBetTypeAndMatch/Index?matchid={MatchId}bettype={BetType}";
        if (IsLive.HasValue)
        {
            link += "&islive=" + IsLive;
        }
        if (!string.IsNullOrWhiteSpace(BetTeam))
        {
            link += "&betteam=" + BetTeam;
        }
        output.Attributes.Add("href", link);

        if (Value.HasValue)
        {
            output.AddClass("fcnum", HtmlEncoder.Default);

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
            output.Content.SetHtmlContent(Text);
        }
    }
}