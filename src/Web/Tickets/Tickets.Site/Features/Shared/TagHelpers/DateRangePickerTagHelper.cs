using System.Text;
using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("daterange-picker")]
public class DateRangePickerTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public DateRangePickerViewModel Model { get; set; }

    [HtmlAttributeName("from-label")]
    public string FromLabel { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;
        var sb = new StringBuilder();
        sb
            .Append("<div id=\"daterange-picker\" class=\"daterange-picker\" ")
            .Append($"data-currentdate=\"{Model.Today}\" ")
            .Append($"data-mindate=\"{Model.MinDate}\" ")
            .Append($"data-maxdate=\"{Model.MaxDate}\"></div>");
        sb
            .Append("<div class=\"form-group from-date-box\">")
            .Append($"<label>{FromLabel ?? Resources.From}</label>")
            .Append("<div class=\"control date\">")
            .Append("<div id=\"from-date-container\" class=\"input-group date fromdate\">")
            .Append("<input type=\"text\" id=\"from-date\" name=\"FromDate\" readonly=\"readonly\" autocomplete=\"off\" ")
            .Append("class=\"ui_nexdatepicker_datetextbox ui_nexdatepicker_fromdate form-control\" ")
            .Append($"value=\"{Model.FromDate}\"/>")
            .Append("<span class=\"input-group-addon\"><span class=\"icon-calendar\" id=\"fdate_trigger\"></span></span>")
            .Append("</div></div></div>");
        sb
            .Append("<div class=\"form-group to-date-box\">")
            .Append($"<label>{Resources.To}</label>")
            .Append("<div class=\"control date\">")
            .Append("<div id=\"to-date-container\" class=\"input-group date todate\">")
            .Append("<input type=\"text\" id=\"to-date\" name=\"ToDate\" readonly=\"readonly\" autocomplete=\"off\" ")
            .Append("class=\"ui_nexdatepicker_datetextbox ui_nexdatepicker_todate form-control\" ")
            .Append($"value=\"{Model.ToDate}\"/>")
            .Append("<span class=\"input-group-addon\"><span class=\"icon-calendar\" id=\"tdate_trigger\"></span></span>")
            .Append("</div></div></div>");

        if (Model.IsRenderSubmitButton)
        {
            sb
                .Append("<div class=\"form-group submit-container\">")
                .Append($"<input type=\"submit\" value=\"{Resources.Submit}\" class=\"btn-submit\"/>")
                .Append("</div>");
        }

        sb
            .Append("<div class=\"form-group daterange-container\">")
            .Append("<div class=\"dropdown daterange-dropdown\" id=\"dropdown-daterange\">")
            .Append($"<input type=\"hidden\" name=\"UserSelectedDateRangeOption\" id=\"hidden-range-option\" value=\"{Model.SelectedRangeOption.Value}\"/>")
            .Append("<button class=\"btn-submit dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">")
            .Append($"<span id=\"range-label\">{Model.SelectedRangeOption.Text}</span>")
            .Append("<span class=\"caret\"></span>")
            .Append("</button>")
            .Append("<ul class=\"dropdown-menu\" aria-labelledby=\"range-label\">");
        foreach (var rangeOption in Model.RangeOptions)
        {
            sb.Append($"<li><a href=\"javascript:;\" class=\"range-option\" data-value=\"{rangeOption.Value}\">{rangeOption.Text}</a></li>");
        }

        sb.Append("</ul></div></div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
