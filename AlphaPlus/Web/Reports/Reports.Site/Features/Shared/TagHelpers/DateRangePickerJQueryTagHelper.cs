using System.Text;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("daterange-picker-jquery")]
public class DateRangePickerJQueryTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public DateRangePickerJQueryViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "form-group daterangepicker-container hide");

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.DateRange}</label>")
            .Append("<div class=\"input-group date datepicker\">")
            .Append("<input type=\"text\" class=\"form-control\" id=\"daterange-picker\" autocomplete=\"off\" readonly ")
            .Append($"data-mindate=\"{Model.MinDate.ToShortDateWithSlash()}\" ")
            .Append($"data-maxdate=\"{Model.MaxDate}\" ")
            .Append($"data-fromdate=\"{Model.FromDate}\" ")
            .Append($"data-todate=\"{Model.ToDate}\"/>")
            .Append("<label class=\"input-group-addon\" for=\"daterange-picker\">")
            .Append("<span class=\"icon-calendar\"></span>")
            .Append("</label></div>");
        var customShortcuts = Model.CustomShortcuts;
        if (customShortcuts.Any())
        {
            sb.Append($"<input type=\"hidden\" id=\"daterange-customshortcuts\" value=\"{JsonHelper.ToJson(customShortcuts)}\"/>");
        }

        sb.Append("</div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}