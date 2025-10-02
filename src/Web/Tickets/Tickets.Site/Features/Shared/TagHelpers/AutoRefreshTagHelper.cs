using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("auto-refresh")]
public class AutoRefreshTagHelper : TagHelper
{
    [HtmlAttributeName("seconds")]
    public int[] Seconds { get; set; }

    [HtmlAttributeName("selected")]
    public int Selected { get; set; }

    [HtmlAttributeName("is-start")]
    public bool IsStart { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("form-group", HtmlEncoder.Default);

        var autoRefresh = new StringBuilder();
        autoRefresh
            .Append($"<div class=\"btn-group-refresh{(IsStart ? string.Empty : " off")}\" id=\"auto-refresh\">")
            .Append("<button type=\"button\" class=\"button-refresh\" id=\"btn-refresh\" style=\"padding: 0 3px; margin-right: 3px;\">")
            .Append("<span class=\"ic-reload\"></span>")
            .Append("</button>")
            .Append("<button type=\"button\" class=\"button-refresh\" id=\"btn-status\" style=\"padding: 0 3px; min-width: 28px;\">")
            .Append("<span id=\"lbl-status-text\" data-running=\"\"></span>")
            .Append("</button>")
            .Append("<button type=\"button\" class=\"button-refresh dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">")
            .Append("<span class=\"countdown\">")
            .Append($"<span id=\"lbl-count-down\" data-original=\"{Selected}\">{Selected}</span>")
            .Append("<span class=\"lds-spinner\">")
            .Append("<span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span>")
            .Append("</span></span>")
            .Append("<span class=\"caret\"></span><span class=\"sr-only\"></span>")
            .Append("</button>")
            .Append("<ul class=\"dropdown-menu\">");
        foreach (var item in Seconds)
        {
            autoRefresh.Append($"<li class=\"auto-refresh-menu-item\"><a href=\"javascript:;\">{item}</a></li>");
        }

        autoRefresh.Append("</ul></div>");

        output.Content.SetHtmlContent(autoRefresh.ToString());
    }
}