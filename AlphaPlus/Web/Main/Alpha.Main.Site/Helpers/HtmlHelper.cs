using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alpha.Main.Site.Helpers;

public static class HtmlHelper
{
    public static IHtmlContent NumericRow(this IHtmlHelper htmlHelper, string label, decimal value, bool canViewReports = false, string url = "", string id = "")
    {
        return new HtmlString($@"
            <div class='row'>
                <div class='col-xs-7'>{label}</div>
                <div class='col-xs-5 value {(value < 0 ? "text-red" : string.Empty)}'>
                    {(canViewReports
                        ? $"<a id='{id}' href='{url}' class='btn-link'>{value.ToString("#,##0.00")}</a>"
                        : $"<strong>{value.ToString("#,##0.00")}</strong>")}
                </div>
            </div>
        ");
    }
}
