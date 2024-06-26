using System.Text;
using System.Text.Encodings.Web;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("week")]
public class WeekTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public IEnumerable<WinLossWeekViewModel> Weeks { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("label-week-box", HtmlEncoder.Default);
        output.Attributes.Add("id", "week-container");

        var week = new StringBuilder();
        foreach (var item in Weeks)
        {
            if (item.IsEnable)
            {
                week.AppendFormat(
                    "<a href=\"javascript:;\" class=\"label-week{0}\" data-fromdate=\"{1}\" data-todate=\"{2}\">{3}</a>",
                    item.IsActive ? " active" : string.Empty,
                    item.FromDate,
                    item.ToDate,
                    item.Display);
            }
            else
            {
                week.Append($"<a href=\"javascript:;\" class=\"label-week disabled\">{item.Display}</a>");
            }
        }

        output.Content.SetHtmlContent(week.ToString());
    }
}
