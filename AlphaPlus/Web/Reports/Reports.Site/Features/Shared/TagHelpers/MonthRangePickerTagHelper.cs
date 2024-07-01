using System.Text;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("month-range-picker")]
public class MonthRangePickerTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public MonthPickerViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var fromDate = Model.FromDate.ToShortDateWithSlash();
        var toDate = Model.ToDate.ToShortDateWithSlash();
        var today = Model.ToDate.ToShortDateWithSlash();
        var minDate = Model.MinDate.ToShortDateWithSlash();
        var maxDate = Model.MaxDate.ToShortDateWithSlash();

        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        var sb = new StringBuilder();
        sb
            .Append("<div class=\"form-group\">")
            .Append($"<label>{Resources.From}</label>")
            .Append("<div class=\"control month-range-picker-element\">")
            .Append("<div id=\"from-monthpicker\"")
            .Append($"data-longmonths-inyear=\"{Model.MonthNames}\" ")
            .Append($"data-mindate=\"{minDate}\" ")
            .Append($"data-maxdate=\"{maxDate}\" ")
            .Append($"data-dateinmonth=\"{fromDate}\" ")
            .Append($"data-currentdate=\"{today}\">")
            .Append("</div></div></div>");
        sb
            .Append("<div class=\"form-group\">")
            .Append($"<label>{Resources.To}</label>")
            .Append("<div class=\"control month-range-picker-element\">")
            .Append("<div id=\"to-monthpicker\"")
            .Append($"data-longmonths-inyear=\"{Model.MonthNames}\" ")
            .Append($"data-mindate=\"{minDate}\" ")
            .Append($"data-maxdate=\"{maxDate}\" ")
            .Append($"data-dateinmonth=\"{toDate}\" ")
            .Append($"data-currentdate=\"{today}\">")
            .Append("</div></div></div>");
        sb
            .Append($"<input type=\"hidden\" id=\"from-date\" name=\"FromDate\" value=\"{fromDate}\" />")
            .Append($"<input type=\"hidden\" id=\"to-date\" name=\"ToDate\" value=\"{toDate}\" />");

        output.Content.SetHtmlContent(sb.ToString());
    }
}