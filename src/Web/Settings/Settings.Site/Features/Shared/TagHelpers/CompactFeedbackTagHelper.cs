using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Settings.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("compact-feedback")]
public class CompactFeedbackTagHelper : TagHelper
{
    [HtmlAttributeName("fid")]
    public int FunctionId { get; set; }

    [HtmlAttributeName("element-id")]
    public string ElementId { get; set; } = "box-icon-title";

    [HtmlAttributeName("class-name")]
    public string ClassName { get; set; }

    [HtmlAttributeName("like-icon-class")]
    public string LikeIconClass { get; set; } = "icon-like";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", $"box-icon-title {ClassName ?? string.Empty}");
        output.Attributes.SetAttribute("id", ElementId);
        output.Attributes.SetAttribute("data-fid", FunctionId);
        var sb = new StringBuilder();
        sb
            .Append("<div class=\"brust-box\">")
            .Append($"<span class=\"{LikeIconClass}\" id=\"icon-like\" data-title=\"{CommonResx.LikeTooltip}\"></span>")
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
            .Append("</ul></div>")
            .Append($"<span class=\"{LikeIconClass} {LikeIconClass}-rotate\" id=\"icon-dislike\" data-title=\"{CommonResx.DislikeTooltip}\"></span>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
