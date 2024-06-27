namespace AlphaPlus.Settings.Site.Features.MemberInfo.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapExMainPath;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> addQueryStringParamsFunc;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> replaceOrAddQueryStringParamsFunc;

        [ThreadStatic]
        private static Func<string, string> mapBetListPath;

        public static Func<string, IDictionary<string, object>, string> AddQueryStringParamsFunc
        {
            get
            {
                return addQueryStringParamsFunc ?? AddQueryStringParams;
            }

            internal set
            {
                addQueryStringParamsFunc = value;
            }
        }

        public static Func<string, IDictionary<string, object>, string> ReplaceOrAddQueryStringParamsFunc
        {
            get
            {
                return replaceOrAddQueryStringParamsFunc ?? ReplaceOrAddQueryStringParams;
            }

            internal set
            {
                replaceOrAddQueryStringParamsFunc = value;
            }
        }

        public static Func<string, string> MapExMainPath
        {
            get
            {
                return mapExMainPath ?? MicroSites.SettingUrl;
            }

            internal set
            {
                mapExMainPath = value;
            }
        }

        public static Func<string, string, string> GetSiteUrl { get; internal set; } = GetLink;

        public static Func<string, string> MapBetListPath
        {
            get
            {
                if (mapBetListPath != null)
                {
                    return mapBetListPath;
                }

                if (AppSettings.IsStandaloneBetlist)
                {
                    mapBetListPath = MapBetListDomain;
                }
                else
                {
                    mapBetListPath = GetBetListPath;
                }

                return mapBetListPath;
            }

            internal set
            {
                mapBetListPath = value;
            }
        }

        internal static string AddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{link}?{queryString}";
        }

        internal static string ReplaceOrAddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            if (!link.Contains("?"))
            {
                return AddQueryStringParams(link, routeValues);
            }

            foreach (var routeValue in routeValues)
            {
                link = ReplaceOrAddQueryStringParam(link, routeValue.Key, routeValue.Value);
            }

            return link;
        }

        internal static string ReplaceOrAddQueryStringParam(string link, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return link;
            }

            var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
            if (Regex.IsMatch(link, regexParamName, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(link, regexParamName, "$1=" + paramValue, RegexOptions.IgnoreCase);
            }

            return link + $"&{paramName}={paramValue}";
        }

        internal static string GetLink(string action, string currentQuery)
        {
            return MicroSites.ReportsUrl(action + currentQuery);
        }

        internal static string GetBetListPath(string virtualPath = "~/WinLoss/Index")
            => MicroSites.TicketUrl(virtualPath);

        internal static string MapBetListDomain(string virtualPath = "~/")
        {
            var betlistDomain = AppSettings.BetlistDomain;
            var cookieless = string.Empty;

            if (UserSession.Current.IsInternal)
            {
                cookieless = $"/(S({AlphaContext.Current.Request.Cookies[$"DFFKDHFJKLDHFJDH-betlist-{AlphaContext.Current.Session.Id}"]}))";
            }

            return $"{betlistDomain}{cookieless}{virtualPath?.Replace("~", string.Empty)}";
        }
    }
}