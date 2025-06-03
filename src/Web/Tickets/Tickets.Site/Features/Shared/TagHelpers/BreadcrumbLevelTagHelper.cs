using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("breadcrumb-level")]
public class BreadcrumbLevelTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public IEnumerable<Models.Breadcrumb> BreadcrumbLevels { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("breadcrumb-container", HtmlEncoder.Default);

        var breadcrumbLevel = new StringBuilder();
        breadcrumbLevel.Append("<ul class=\"breadcrumb\" id=\"breadcrumb-level\">");
        foreach (var item in BreadcrumbLevels)
        {
            breadcrumbLevel.Append("<li>");
            if (item.CustLevelId > 1)
            {
                breadcrumbLevel.AppendFormat(
                    "<a href=\"javascript:;\" class=\"upline-link\" data-custid=\"{0}\" data-custname=\"{1}\" data-custlevelid=\"{2}\">{3}</a>",
                    item.CustId,
                    item.CustName,
                    item.CustLevelId,
                    item.CustName);
            }
            else
            {
                breadcrumbLevel.Append(item.CustName);
            }

            breadcrumbLevel.Append("</li>");
        }

        breadcrumbLevel.Append("</ul>");

        output.Content.SetHtmlContent(breadcrumbLevel.ToString());
    }
}
