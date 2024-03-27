using System.Text;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("report-title")]
public class ReportTitleTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public PageHeaderViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", $"page-title{(Model.IsStackMode ? " stack-mode" : string.Empty)}{(Model.IsFilterVisible ? string.Empty : " hide-filter")}");

        var sb = new StringBuilder();
        AppendEnteredSportResult(sb);
        sb.Append($"<div class=\"text-title pull-left\" style=\"clear:both\">{Model.Title}</div>");

        AppendBreadcrumbLevel(sb);
        AppendActionIcons(sb);
        AppendNewFeatureNote(sb);

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AppendBreadcrumbLevel(StringBuilder sb)
    {
        var breadcrumbs = Model.BreadcrumbLevel?.Breadcrumbs;

        if (breadcrumbs != null)
        {
            var linkLength = breadcrumbs.Count();

            if (linkLength > 1)
            {
                sb.Append("<ul class=\"breadcrumb\">");

                var linkIndex = 0;
                var lastLinkIndex = linkLength - 1;
                foreach (var item in breadcrumbs)
                {
                    if (linkIndex == lastLinkIndex)
                    {
                        sb.Append("<li>").Append(item.CustName).Append("</li>");
                    }
                    else
                    {
                        sb
                            .Append("<li><a class=\"breadcrumb-link\" href=\"javascript:;\" ")
                            .Append($"data-custid=\"{item.CustId}\" ")
                            .Append($"data-custname=\"{item.CustName}\" ")
                            .Append($"data-custlevelid=\"{item.CustLevelId}\">")
                            .Append(item.CustName)
                            .Append("</a></li>");
                    }

                    linkIndex++;
                }

                sb.Append("</ul>");
            }
        }
    }

    private void AppendEnteredSportResult(StringBuilder sb)
    {
        var total = Model.TotalEnteredResults;
        if (total != null)
        {
            var today = DateTime.Today;
            var yesterday = today.Yesterday();

            sb.Append("<div class=\"entered-non-entered\">");
            if (total.EnteredResultYesterday.NonEntered > 0)
            {
                AppendNonEnteredSportResult(sb, yesterday, total.EnteredResultYesterday.NonEntered);
            }
            else
            {
                sb
                    .Append("<div class=\"icon-entered\">")
                    .Append($"<div class=\"text\">{yesterday.ToMMDDWithSlash()}: {Resources.Entered}</div>")
                    .Append("<div class=\"icon-check-circle\"></div>")
                    .Append("</div>");
            }

            AppendNonEnteredSportResult(sb, today, total.EnteredResultToday.NonEntered);
            sb.Append("</div>");
        }
    }

    private static void AppendNonEnteredSportResult(StringBuilder sb, DateTime date, int nonEntered)
    {
        sb
            .Append("<div class=\"icon-non-entered\">")
            .Append($"<div class=\"text\">{date.ToMMDDWithSlash()}: {Resources.NonEntered}</div>")
            .Append($"<div class=\"number\">{nonEntered}</div>")
            .Append("<div class=\"icon-clock\"></div>")
            .Append("</div>");
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

    private void AppendNewFeatureNote(StringBuilder sb)
    {
        if (Model.HasNewFeatureNote)
        {
            sb
                .Append("<p class=\"note_title\"><span class=\"require\">*</span>&nbsp;&nbsp;")
                .Append(Resources.NewViewTakesOverNote)
                .Append("</p>");
        }
    }
}
