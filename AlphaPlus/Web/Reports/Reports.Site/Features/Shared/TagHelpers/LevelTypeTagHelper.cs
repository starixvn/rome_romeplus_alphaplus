using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("level-type")]
public class LevelTypeTagHelper : TagHelper
{
    [HtmlAttributeName("custlevel")]
    public UserLevel CustLevel { get; set; }

    [HtmlAttributeName("selected-value")]
    public byte SelectedLevel { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "form-group level-type-multiselect");

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.Level}</label>")
            .Append("<select id=\"dropdown-level-type\">")
            .Append($"<option value=\"0\" {(SelectedLevel == 0 ? "selected" : string.Empty)}>{Resources.All}</option>")
            .Append($"<option value=\"1\" {(SelectedLevel == 1 ? "selected" : string.Empty)}>{(CustLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent)}</option>")
            .Append($"<option value=\"2\" {(SelectedLevel == 2 ? "selected" : string.Empty)}>{Resources.DirectMember}</option>")
            .Append("</select>")
            .Append($"<input type=\"hidden\" name=\"LevelType\" id=\"level-type\" value=\"{SelectedLevel}\" />");

        output.Content.SetHtmlContent(sb.ToString());
    }
}