using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Common;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text.Encodings.Web;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("compact-feedback")]
public class CompactFeedbackTagHelper : TagHelper
{
    [HtmlAttributeName("fid")]
    public int FunctionId { get; set; }

    [HtmlAttributeName("bootstrap-tooltip")]
    public bool IsBootstrapTooltip { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("box-icon-title", HtmlEncoder.Default);
        output.Attributes.SetAttribute("id", "box-icon-title");
        output.Attributes.SetAttribute("data-fid", FunctionId);
        var sb = new StringBuilder();
        sb.Append("<div class=\"brust-box\">");
        if (IsBootstrapTooltip)
        {
            sb.Append($"<span class=\"ic-like\" id=\"icon-like\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{CommonResx.LikeTooltip}\"></span>");
        }
        else
        {
            sb.Append($"<span class=\"ic-like\" id=\"icon-like\" data-title=\"{CommonResx.LikeTooltip}\"></span>");
        }
        sb
            .Append("<ul class=\"bursts\">")
            .Append("<li class=\"deg0\"></li>")
            .Append("<li class=\"deg36\"></li>")
            .Append("<li class=\"deg72\"></li>")
            .Append("<li class=\"deg108\"></li>")
            .Append("<li class=\"deg144\"></li>")
            .Append("<li class=\"deg180\"></li>")
            .Append("<li class=\"deg216\"></li>")
            .Append("<li class=\"deg252\"></li>")
            .Append("<li class=\"deg288\"></li>")
            .Append("<li class=\"deg324\"></li>")
            .Append("</ul></div>");
        if (IsBootstrapTooltip)
        {
            sb.Append($"<span class=\"ic-like ic-like-rotate\" id=\"icon-dislike\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{CommonResx.DislikeTooltip}\"></span>");
        }
        else
        {
            sb.Append($"<span class=\"ic-like ic-like-rotate\" id=\"icon-dislike\" data-title=\"{CommonResx.DislikeTooltip}\"></span>");
        }

        output.Content.SetHtmlContent(sb.ToString());
    }
}
