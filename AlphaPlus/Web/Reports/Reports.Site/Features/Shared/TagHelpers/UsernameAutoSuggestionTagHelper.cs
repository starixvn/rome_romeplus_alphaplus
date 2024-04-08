using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("username-autosuggestion")]
public class UsernameAutoSuggestionTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("form-group", HtmlEncoder.Default);

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.Username}</label>")
            .Append("<div>")
            .Append($"<input type=\"text\" id=\"txt-username-auto-suggestion\" placeholder=\"${Resources.UsernameOrFirstLastName}\" autocomplete=\"off\"/>")
            .Append("</div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
