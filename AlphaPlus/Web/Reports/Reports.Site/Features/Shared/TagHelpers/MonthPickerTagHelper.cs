using System.Text;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("month-picker")]
public class MonthPickerTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public MonthPickerViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var today = DateTime.Today;
        var maxDate = Model.IsViewNextYear ? today.AddYears(1).LastDayOfYear() : today;

        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        var sb = new StringBuilder();
        sb
            .Append("<div class=\"form-group monthpicker-container\">")
            .Append("<div id=\"monthPickerID\">")
            .Append("<div id=\"dropdown-month\" ")
            .Append($"data-longmonths-inyear=\"{Model.MonthNames}\" ")
            .Append($"data-mindate=\"{Model.MinDate.ToShortDateWithSlash()}\" ")
            .Append($"data-maxdate=\"{maxDate.ToShortDateWithSlash()}\" ")
            .Append($"data-dateinmonth=\"{Model.FromDate.ToShortDateWithSlash()}\" ")
            .Append($"data-fromdate=\"{Model.FromDate.Day}\" ")
            .Append($"data-todate=\"{Model.ToDate.Day}\" ")
            .Append($"data-currentdate=\"{today.ToShortDateWithSlash()}\">")
            .Append("</div></div></div>")
            .Append($"<input type=\"hidden\" id=\"from-date\" name=\"FromDate\" value=\"{Model.FromDate}\" />")
            .Append($"<input type=\"hidden\" id=\"to-date\" name=\"ToDate\" value=\"{Model.ToDate}\" />");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
