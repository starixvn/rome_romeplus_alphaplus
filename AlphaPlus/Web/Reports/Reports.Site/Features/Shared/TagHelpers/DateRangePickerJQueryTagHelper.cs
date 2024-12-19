using System.Text;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

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
        output.Attributes.Add("class", "form-group daterangepicker-container");

        var sb = new StringBuilder();
        sb
            .Append($"<label class=\"label-form-control\">{Resources.DateRange}</label>")
            .Append("<div class=\"input-group date datepicker\">")
            .Append("<input type=\"text\" class=\"form-control\" id=\"daterange-picker\" autocomplete=\"off\" readonly ")
            .Append($"data-mindate=\"{Model.MinDate}\" ")
            .Append($"data-maxdate=\"{Model.MaxDate}\" ")
            .Append($"data-fromdate=\"{Model.FromDate}\" ")
            .Append($"data-todate=\"{Model.ToDate}\" ");
        AddCustomShortcuts(sb);
        sb.Append($"data-language=\"{Model.Language}\"/>")
            .Append("<label class=\"input-group-addon\" for=\"daterange-picker\">")
            .Append("<span class=\"icon-calendar\"></span>")
            .Append("</label></div>");

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AddCustomShortcuts(StringBuilder sb)
    {
        if (Model.ShowShortcuts)
        {
            sb.Append($"data-today=\"{Model.Today}\" ");

            foreach (var item in Model.CustomShortcuts)
            {
                if (item == DateRangePickerJQueryViewModel.TodayOptionValue)
                {
                    sb.Append($"data-labeltoday=\"{Resources.Today}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.YesterdayOptionValue)
                {
                    sb.Append($"data-labelyesterday=\"{Resources.Yesterday}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.ThisWeekOptionValue)
                {
                    sb.Append($"data-labelthisweek=\"{Resources.ThisWeek}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.LastWeekOptionValue)
                {
                    sb.Append($"data-labellastweek=\"{Resources.LastWeek}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.ThisMonthOptionValue)
                {
                    sb.Append($"data-labelthismonth=\"{Resources.ThisMonth}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.LastMonthOptionValue)
                {
                    sb.Append($"data-labellastmonth=\"{Resources.LastMonth}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.SinceLastMonthOptionValue)
                {
                    sb.Append($"data-labelsincelastmonth=\"{Resources.SinceLastMonth}\" ");
                }
                else if (item == DateRangePickerJQueryViewModel.Last3MonthsOptionValue)
                {
                    sb.Append($"data-labellast3months=\"{Resources.Last3Months}\" ");
                }
            }
        }
    }
}