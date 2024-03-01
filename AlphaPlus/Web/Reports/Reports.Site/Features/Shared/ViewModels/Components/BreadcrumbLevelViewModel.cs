using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

public class BreadcrumbLevelViewModel(
    IEnumerable<Breadcrumb> breadcrumbLevel = null,
    string breadcrumbLevelJson = null,
    IEnumerable<string> breadcrumbParamByLevel = null,
    string requestQuery = null)
{
    public IEnumerable<Breadcrumb> BreadcrumbLevel { get; } = breadcrumbLevel ?? [];

    public string BreadcrumbLevelJson { get; } = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);

    public string BuildBreadcrumbLink(Breadcrumb model, int breadcrumbIndex)
    {
        var url = breadcrumbParamByLevel == null
            ? requestQuery
            : $"{breadcrumbParamByLevel.ElementAt(breadcrumbIndex)}{requestQuery}";

        return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
            url,
            [
                KeyValuePair.Create(nameof(model.CustId), model.CustId.ToString()),
                KeyValuePair.Create(nameof(model.CustName), model.CustName),
                KeyValuePair.Create(nameof(model.CustLevelId), model.CustLevelId.ToString()),
                KeyValuePair.Create(nameof(BreadcrumbLevelJson), BreadcrumbLevelJson)
            ]);
    }
}