using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
using AlphaPlus.Settings.Site.ViewModels.Components;

namespace AlphaPlus.Settings.Site.MemberInfo.Helpers;

public static class Pager
{
    public static string Render(PagerViewModel model)
    {
        if (model?.TotalRecords > 0)
        {
            return @"<div id='report-pager' " + @"
                data-labelpage='" + Resources.Page + @"'
                data-labelof='" + Resources.Of + @"'
                data-labeldisplayitems='" + Resources.Pager_DisplayItems + @"'
                data-pageindex='" + model.PageIndex + @"'
                data-pagesize='" + model.PageSize + @"'
                data-totalrecords='" + model.TotalRecords + @"'>
            </div>";
        }

        return string.Empty;
    }
}

public static class NoInformation
{
    public static string Render(int colspan)
    {
        return @"<tbody>
            <tr>
                <td colspan='" + colspan + "' align='center'>" + Resources.NoInformationIsAvailable + @"</td>
            </tr>
        </tbody>";
    }
}

public static class HtmlHelpers
{
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