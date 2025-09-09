using System.Text;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("no-infomation")]
public class NoInfomationTagHelper : TagHelper
{
    [HtmlAttributeName("colspan")]
    public int Colspan { get; set; }

    [HtmlAttributeName("hastbody")]
    public bool HasTBody { get; set; } = true;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var sb = new StringBuilder();
        if (HasTBody)
        {
            output.TagName = "tbody";
            sb.Append("<tr>");
        }
        else
        {
            output.TagName = "tr";
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        sb.Append($"<td colspan=\"{Colspan}\" class=\"text-center\" data-noinfo=\"1\">{Resources.NoInformationIsAvailable}</td>");

        if (HasTBody)
        {
            sb.Append("</tr>");
        }

        output.Content.SetHtmlContent(sb.ToString());
    }
}