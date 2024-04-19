using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("visible-column")]
public class VisibleColumnTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("form-group", HtmlEncoder.Default);

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.Columns}</label>")
            .Append("<div id=\"dropdown-visible-column\"")
            .Append($"data-label-selectall=\"{Resources.SelectAll}\" ")
            .Append($"data-label-selected=\"{Resources.nSelected}\" ")
            .Append($"data-label-noneselected=\"{Resources.NoneSelected}\" ")
            .Append($"data-label-all=\"{Resources.All}\" ")
            .Append($"data-label-noitemfound=\"{Resources.NoColumnsFound}\" ")
            .Append($"data-label-filterplaceholder=\"{Resources.SearchColumns}\"></div></div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}