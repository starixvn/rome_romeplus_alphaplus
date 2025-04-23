using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("username-autosuggestion")]
public class UsernameAutoSuggestionTagHelper : TagHelper
{
    [HtmlAttributeName("value")]
    public string Value { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "form-group form-group-username");

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.Username}</label>")
            .Append("<div>")
            .Append($"<input type=\"text\" id=\"txt-username-suggestion\" placeholder=\"{Resources.UsernameOrFirstLastName}\" autocomplete=\"off\" maxlength=\"50\" value=\"{Value}\"/>")
            .Append("</div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
