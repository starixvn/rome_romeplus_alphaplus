using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
using AlphaPlus.Settings.Site.Features.Shared.ViewModels;
using AlphaPlus.Settings.Site.ViewModels.Components;
using Microsoft.AspNetCore.Html;

namespace AlphaPlus.Settings.Site.MemberInfo.Helpers;

public static class Breadcrumb
{
    public static IHtmlContent Render(BreadcrumbLevelViewModel viewModel, string currentRequestUrl, IEnumerable<string> breadcrumbParamByLevel)
    {
        var linkLength = viewModel.BreadcrumbLevel != null ? viewModel.BreadcrumbLevel.Count() : 0;
        var linkIndex = 0;
        var lastLinkIndex = linkLength - 1;

        if (linkLength > 0)
        {
            var breadcrumb = @"<div class='breadcrumb-container'>
                    <ul class='breadcrumb'>";

            foreach (var custinfo in viewModel.BreadcrumbLevel)
            {
                if (linkIndex == lastLinkIndex)
                {
                    breadcrumb += "<li>" + custinfo.CustName + "</li>";
                }
                else
                {
                    breadcrumb += @"
                        <li>
                            <a class='breadcrumb-link' 
                             href='" + viewModel.BuildBreadcrumbLink(custinfo, currentRequestUrl, breadcrumbParamByLevel, linkIndex) + "'>"
                                + custinfo.CustName +
                            "</a>" +
                        "</li>";
                }

                linkIndex++;
            }

            breadcrumb += "</ul></div>";

            return new HtmlString(breadcrumb);
        }

        return HtmlString.Empty;
    }
}

public static class Alert{
    public static IHtmlContent Render()
    {
        return new HtmlString(
            @"<div id='alert-message' class='msg-content hidden'>
                <div id='alert-message-content'></div>
            </div>");
    }
}

public static class Pager
{
    public static IHtmlContent Render(PagerViewModel model)
    {
        if (model?.TotalRecords > 0)
        {
            return new HtmlString(@"<div id='report-pager' " + @"
                data-labelpage='" + Resources.Page + @"'
                data-labelof='" + Resources.Of + @"'
                data-labeldisplayitems='" + Resources.Pager_DisplayItems + @"'
                data-pageindex='" + model.PageIndex + @"'
                data-pagesize='" + model.PageSize + @"'
                data-totalrecords='" + model.TotalRecords + @"'>
            </div>");
        }

        return HtmlString.Empty;
    }
}

public static class NoInformation
{
    public static IHtmlContent Render(int colspan)
    {
        return new HtmlString(@"<tbody>
            <tr>
                <td colspan='" + colspan + "' align='center'>" + Resources.NoInformationIsAvailable + @"</td>
            </tr>
        </tbody>");
    }
}

public static class HtmlHelpers
{
    public static string FormatNumber2(decimal value)
    {
        return value.ToString($"#,##0.##");
    }
    public static string FormatNumber(decimal value)
    {
        return value.ToString($"#,##0.00");
    }

    public static string FormatNumber(int value)
    {
        return value.ToString($"#,##0");
    }

    public static string RenderNumberCell(decimal cellValue, string cellClass = "")
    {
        return @"
            <td class='" + (cellValue < 0 ? "negative-value" : string.Empty) + " " + cellClass + "'>" +
                FormatNumber(cellValue) +
            "</td>";
    }

    public static string FormatDecimal(decimal value, bool showRedColorForNegativeValue = false)
    {
        if (showRedColorForNegativeValue && value < 0)
        {
            return "<span class='negative-value'>" + value.ToString("#,##0.##") + "</span>";
        }
        else
        {
            return value.ToString("#,##0.##");
        }
    }

    public static string FormatThreshold(decimal? number)
    {
        if (number.HasValue)
        {
            return FormatDecimal(number.Value);
        }
        else
        {
            return Resources.Unlimited;
        }
    }
}