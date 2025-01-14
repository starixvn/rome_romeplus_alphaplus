using System.Text;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Core.Helpers;

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
        output.Attributes.Add("class", $"page-title {(Model.IsStackMode ? " stack-mode" : string.Empty)}{(Model.IsFilterVisible ? string.Empty : " hide-filter")}");

        var sb = new StringBuilder();
        AppendEnteredSportResult(sb);
        sb.Append($"<div class=\"text-title\" style=\"clear:both\">{Model.Title}</div>");

        AppendBreadcrumbLevel(sb);

        if (!string.IsNullOrWhiteSpace(Model.IconInfoUrl))
        {
            sb.Append($"<a href=\"{Model.IconInfoUrl}\" target=\"_blank\"><span class=\"icon-information icon icon-information-outline\"></span></a>");
        }

        AppendActionIcons(sb);

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AppendBreadcrumbLevel(StringBuilder sb)
    {
        if (AlphaContext.Current?.Request?.Query?.Count > 0)
        {
            var breadcrumbs = Model.BreadcrumbLevel?.Breadcrumbs;

            if (breadcrumbs != null)
            {
                var linkLength = breadcrumbs.Count();

                if (linkLength > 0)
                {
                    sb.Append("<div class=\"breadcrumb-container\"><ul class=\"breadcrumb\">");
                    var linkIndex = 0;
                    var lastLinkIndex = linkLength - 1;
                    foreach (var item in breadcrumbs)
                    {
                        if (linkIndex == lastLinkIndex)
                        {
                            sb.Append($"<li>&nbsp{item.CustName}&nbsp</li>");
                        }
                        else
                        {
                            sb
                                .Append("<li><a class=\"breadcrumb-link\" href=\"javascript:;\" ");
                            if (item.IsConsolidation)
                            {
                                sb.Append($"data-consolidation=\"true\" ");
                            }
                            sb
                                .Append($"data-custid=\"{item.CustId}\" ")
                                .Append($"data-custname=\"{item.CustName}\" ")
                                .Append($"data-custlevelid=\"{item.CustLevelId}\">")
                                .Append("&nbsp")
                                .Append(item.CustName)
                                .Append("&nbsp")
                                .Append("</a></li>");
                        }

                        linkIndex++;
                    }

                    sb.Append("</ul></div>");
                }
            }
        }
    }

    private void AppendEnteredSportResult(StringBuilder sb)
    {
        var total = Model.TotalEnteredResults;
        if (total != null)
        {
            var yesterday = total.EnteredResultYesterday.EventDateText;
            sb.Append("<div class=\"entered-non-entered\">");
            if (total.EnteredResultYesterday.NonEntered > 0)
            {
                AppendNonEnteredSportResult(sb, yesterday, total.EnteredResultYesterday.NonEntered);
            }
            else
            {
                sb
                    .Append("<div class=\"icon-entered\">")
                    .Append($"<div class=\"text\">{yesterday}: {Resources.Entered}</div>")
                    .Append("<div class=\"icon-check-circle\"></div>")
                    .Append("</div>");
            }

            AppendNonEnteredSportResult(sb, total.EnteredResultToday.EventDateText, total.EnteredResultToday.NonEntered);
            sb.Append("</div>");
        }
    }

    private static void AppendNonEnteredSportResult(StringBuilder sb, string formatedDate, int nonEntered)
        => sb
        .Append("<div class=\"icon-non-entered\">")
        .Append($"<div class=\"text\">{formatedDate}: {Resources.NonEntered}</div>")
        .Append($"<div class=\"number\">{nonEntered}</div>")
        .Append("<div class=\"icon-clock\"></div>")
        .Append("</div>");

    private void AppendActionIcons(StringBuilder sb)
    {
        if (Model.IsShowActionIcons)
        {
            sb.Append("<div class=\"list-icon\">").Append("<ul>");

            if (Model.HasHideFilterIcon)
            {
                sb
                    .Append("<li class=\"showfilter-element\"><span class=\"ic-arrow-up-circle icon-filter\" title=\"")
                    .Append(Resources.HideFilter)
                    .Append("\"></span></li>")
                    .Append("<li class=\"hidefilter-element\"><span class=\"ic-arrow-down-circle icon-filter\" title=\"")
                    .Append(Resources.ShowFilter)
                    .Append("\"></span></li>");
            }

            if (Model.HasStackIcon)
            {
                sb
                    .Append("<li class=\"normalview-element\"><span id=\"icon-stack-column\" class=\"ic-stack-column ic-switchmode\" title=\"")
                    .Append(Resources.Tip_ShowReportInStackColumnView)
                    .Append("\"></span></li>")
                    .Append("<li class=\"stackcolumnview-element\"><span id=\"icon-normal-column\" class=\"ic-normal-column ic-switchmode\" title=\"")
                    .Append(Resources.Tip_ShowReportInNormalView)
                    .Append("\"></span></li>");
            }

            sb.Append("</ul>").Append("</div>");
        }
    }
}
