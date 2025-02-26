using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("number")]
public class NumberTagHelper : TagHelper
{
    [HtmlAttributeName("value")]
    public decimal Value { get; set; }

    [HtmlAttributeName("redpositive")]
    public bool? RedPositive { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        var formatedValue = Value.ToString("N0");
        if (RedPositive.HasValue
        && ((RedPositive == true && Value > 0) || (RedPositive == false && Value < 0)))
        {
            output.AddClass("positive", HtmlEncoder.Default);
        }

        output.Content.SetHtmlContent(formatedValue);

    }
}