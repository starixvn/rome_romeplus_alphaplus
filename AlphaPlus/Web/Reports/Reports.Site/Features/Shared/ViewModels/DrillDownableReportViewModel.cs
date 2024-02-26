using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels;

public abstract class DrillDownableReportViewModel<T> where T : class
{
    protected DrillDownableReportViewModel(
        UserLevel custLevel,
        IEnumerable<Breadcrumb> breadcrumbs,
        string defaultBetListPath = null)
    {
        CustLevel = custLevel;
        Breadcrumbs = breadcrumbs;
        BreadcrumbsJson = JsonHelper.ToJsonFunc(breadcrumbs);
        BetListPath = defaultBetListPath;
    }

    public UserLevel CustLevel { get; }

    public IEnumerable<Breadcrumb> Breadcrumbs { get; }

    public string BreadcrumbsJson { get; }

    protected string BetListPath { get; set; }

    public virtual string BuildDrillDownLink(T model, string currentLink)
        => CustLevel.IsAgentLevel()
        ? BuildBetListLink(model)
        : BuildDownlineLink(model, currentLink);

    protected virtual string BuildDownlineLink(T model, string currentLink)
    {
        var queryParams = GetDownlineLinkQueryParams(model);

        return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
            currentLink,
            queryParams.Concat(
            [
                KeyValuePair.Create("CustLevelId", CustLevel.GetDirectDownlineLevel().Value.ToString()),
                KeyValuePair.Create("BreadcrumbLevelJson", BreadcrumbsJson)
            ]));
    }

    protected virtual string BuildBetListLink(T model)
        => UrlHelper.AddQueryStringParamsFunc(
        BetListPath,
        GetBetListLinkQueryParams(model));

    protected abstract IEnumerable<KeyValuePair<string, string>> GetDownlineLinkQueryParams(T model);

    protected abstract IEnumerable<KeyValuePair<string, string>> GetBetListLinkQueryParams(T model);
}
