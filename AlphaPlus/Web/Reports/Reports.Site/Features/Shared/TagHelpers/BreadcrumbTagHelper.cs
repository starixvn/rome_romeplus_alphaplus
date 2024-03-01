using System.Text;
using System.Text.Encodings.Web;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

public class BreadcrumbTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public BreadcrumbLevelViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("breadcrumb-container", HtmlEncoder.Default);

        var sb = new StringBuilder();
        sb.Append("<ul class=\"breadcrumb\">");

        var linkLength = Model.BreadcrumbLevel.Count();
        var linkIndex = 0;
        var lastLinkIndex = linkLength - 1;
        foreach (var item in Model.BreadcrumbLevel)
        {
            if (linkIndex == lastLinkIndex)
            {
                sb.Append("<li>").Append(item.CustName).Append("</li>");
            }
            else
            {
                sb
                    .Append("<li><a class=\"breadcrumb-link\"> href=\"")
                    .Append(Model.BuildBreadcrumbLink(item, linkIndex))
                    .Append("\">")
                    .Append(item.CustName)
                    .Append("</a></li>");
            }

            linkIndex++;
        }

        sb
            .Append("</ul>")
            .Append("<script type=\"text/javascript\"> ")
            .Append("try { (function ($) { ")
            .Append("$.each($('.breadcrumb li'), function (index, element) { ")
            .Append("$(element).text($(element).text().trim()); ")
            .Append("}); })(window.jQuery || window.parent.jQuery || window.top.jQuery); ")
            .Append("} catch (e) {} ")
            .Append("</script>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}