using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Settings.Site.Helpers;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    public class BreadcrumbLevelViewModel(IEnumerable<CustInfo> breadcrumbLevel, string breadcrumbLevelJson = null)
    {
        public IEnumerable<CustInfo> BreadcrumbLevel { get; } = breadcrumbLevel;

        public string BreadcrumbLevelJson { get; } = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);

        public string BuildBreadcrumbLink(CustInfo model, string currentLink, IEnumerable<string> breadcrumbParamByLevel, int breadcrumbIndex)
        {
            var url = breadcrumbParamByLevel == null
                ? currentLink
                : UrlHelper.GetSiteUrl(breadcrumbParamByLevel.ElementAt(breadcrumbIndex), currentLink);

            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                url,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", model.CustLevelId },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }
    }
}