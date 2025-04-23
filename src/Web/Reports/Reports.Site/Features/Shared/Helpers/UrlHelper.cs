using System.Text.RegularExpressions;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class UrlHelper
{
    public static Func<string, IEnumerable<KeyValuePair<string, string>>, string> AddQueryStringParamsFunc { get; } = AddQueryStringParams;

    internal static string AddQueryStringParams(string link, IEnumerable<KeyValuePair<string, string>> queryParams)
        => queryParams.Any()
        ? link + QueryString.Create(queryParams)
        : link;

    public static Func<string, IEnumerable<KeyValuePair<string, string>>, string> ReplaceOrAddQueryStringParamsFunc { get; } = ReplaceOrAddQueryStringParams;

    internal static string ReplaceOrAddQueryStringParams(string link, IEnumerable<KeyValuePair<string, string>> queryParams)
    {
        if (!queryParams.Any())
        {
            return link;
        }

        if (!link.Contains("?"))
        {
            return AddQueryStringParams(link, queryParams);
        }

        foreach (var (key, value) in queryParams)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                link = ReplaceOrAddQueryStringParam(link, key, value);
            }
        }

        return link;
    }

    internal static string ReplaceOrAddQueryStringParam(string link, string paramName, string paramValue)
    {
        var regexParamName = @"([?&]" + paramName + ")=[^?&]+";

        if (Regex.IsMatch(link, regexParamName, RegexOptions.IgnoreCase))
        {
            return Regex.Replace(link, regexParamName, "$1=" + paramValue, RegexOptions.IgnoreCase);
        }

        return link + $"&{paramName}={paramValue}";
    }
}