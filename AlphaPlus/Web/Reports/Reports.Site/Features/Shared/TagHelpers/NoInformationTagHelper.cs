using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("no-information")]
public class NoInformationTagHelper : TagHelper
{
    [HtmlAttributeName("colspan")]
    public int Colspan { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tbody";
        output.TagMode = TagMode.StartTagAndEndTag;

        var html = $"<tr><td colspan=\"{Colspan}\" class=\"text-center first-row\">{Resources.NoInformationIsAvailable}</td></tr>";
        output.Content.SetHtmlContent(html);
    }
}
