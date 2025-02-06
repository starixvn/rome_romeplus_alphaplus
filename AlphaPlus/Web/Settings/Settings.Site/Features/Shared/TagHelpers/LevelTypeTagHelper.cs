using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Settings.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("level-type")]
public class LevelTypeTagHelper : TagHelper
{
    [HtmlAttributeName("custlevel")]
    public UserLevel CustLevel { get; set; }

    [HtmlAttributeName("selected-value")]
    public byte SelectedLevel { get; set; }

    [HtmlAttributeName("show-all-option")]
    public bool ShowAllOption { get; set; } = true;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "form-group level-type-multiselect");

        var sb = new StringBuilder();
        sb
            .Append($"<label>{Resources.Level}</label>")
            .Append("<select id='dropdown-level-type' name='LevelType'>");

        if (ShowAllOption)
        {
            sb.Append($"<option value='0' {(SelectedLevel == 0 ? "selected" : string.Empty)}>{Resources.All}</option>");
        }

        sb.Append($"<option value='0' {(SelectedLevel == 0 ? "selected" : string.Empty)}>{Resources.All}</option>")
        .Append($"<option value='1' {(SelectedLevel == 1 ? "selected" : string.Empty)}>{(CustLevel.IsSuperLevel() ? CommonResx.MasterAgent : Resources.Agent)}</option>")
        .Append($"<option value='2' {(SelectedLevel == 2 ? "selected" : string.Empty)}>{CommonResx.DirectMember}</option>")
        .Append("</select>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
