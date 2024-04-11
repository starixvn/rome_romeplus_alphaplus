using System.Text;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("report-title-new")]
public class ReportTitleNewTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public PageHeaderViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"page-title page-title-entered {(Model.IsStackMode ? " stack-mode" : string.Empty)}{(Model.IsFilterVisible ? string.Empty : " hide-filter")}");

        var sb = new StringBuilder();
        sb
            .Append($"<div class=\"text-title pull-left\" id=\"report-title\">${Model.Title}</div>")
            .Append("<a href=\"javascript:;\"><span class=\"icon icon-information-outline icon-report-legend\" id=\"icon-report-legend\"></span></a>");
        AppendEnteredSportResult(sb);
        AppendActionIcons(sb);

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AppendEnteredSportResult(StringBuilder sb)
    {
        var total = Model.TotalEnteredResults;
        if (total != null)
        {
            var today = DateTime.Today;
            var yesterday = today.Yesterday();
            var hasNonEnteredYerterday = total.EnteredResultYesterday.NonEntered > 0;

            sb.Append("<div class=\"entered-non-entered\">");
            sb
                .Append($"<div class=\"icon-non-entered\" id=\"total-non-entered-yesterday\"${(hasNonEnteredYerterday ? string.Empty : " style=display:none")}>")
                .Append($"<div class=\"text\"><span class=\"event-date-yesterday\">${yesterday.ToMMDDWithSlash()}</span>: ${Resources.NonEntered}</div>")
                .Append($"<div class=\"number\" id=\"total-non-entered-yesterday-number\">{total.EnteredResultYesterday.NonEntered.FormatNumber()}</div>")
                .Append("<div class=\"icon-clock\"></div></div>");
            sb
                .Append($"<div class=\"icon-entered\" id=\"total-entered-yesterday\"${(hasNonEnteredYerterday ? " style=display:none" : string.Empty)}>")
                .Append($"<div class=\"text\"><span class=\"event-date-yesterday\">${yesterday.ToMMDDWithSlash()}</span>: ${Resources.Entered}</div>")
                .Append("<div class=\"icon-check-circle\"></div></div>");
            sb
                .Append($"<div class=\"icon-non-entered\" id=\"total-non-entered-today\">")
                .Append($"<div class=\"text\"><span id=\"total-non-entered-today-text\">${today.ToMMDDWithSlash()}</span>: ${Resources.NonEntered}</div>")
                .Append($"<div class=\"number\" id=\"total-non-entered-today-number\">{total.EnteredResultToday.NonEntered.FormatNumber()}</div>")
                .Append("<div class=\"icon-clock\"></div></div>");
            sb.Append("</div>");
        }
    }

    private void AppendActionIcons(StringBuilder sb)
    {
        if (Model.IsShowActionIcons)
        {
            sb.Append("<div class=\"list-icon\">").Append("<ul>");

            if (Model.HasHideFilterIcon)
            {
                sb
                    .Append("<li class=\"showfilter-element\"><span class=\"icon-arrow-up-drop-circle icon-filter\" title=\"")
                    .Append(Resources.HideFilter)
                    .Append("\"></span></li>")
                    .Append("<li class=\"hidefilter-element\"><span class=\"icon-arrow-down-drop-circle icon-filter\" title=\"")
                    .Append(Resources.ShowFilter)
                    .Append("\"></span></li>");
            }

            if (Model.HasStackIcon)
            {
                sb
                    .Append("<li class=\"normalview-element\"><span id=\"icon-stack-column\" class=\"icon-stack-column icon-switchmode\" title=\"")
                    .Append(Resources.Tip_ShowReportInStackColumnView)
                    .Append("\"></span></li>")
                    .Append("<li class=\"stackcolumnview-element\"><span id=\"icon-normal-column\" class=\"icon-normal-column icon-switchmode\" title=\"")
                    .Append(Resources.Tip_ShowReportInNormalView)
                    .Append("\"></span></li>");
            }

            sb.Append("</ul>").Append("</div>");
        }
    }
}
