namespace AlphaPlus.Settings.Site.ViewLogs.Requests.Credit
{
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.ViewLogs.API.Helpers;
    using MediatR;
    using ViewModels;

    public class IndexQuery : LogQuery, IRequest<LogViewModel>
    {
        public IndexQuery() : this(null, null, null)
        {
        }

        public IndexQuery(
            IUserSession currentUserSession,
            IDateTime dateTimeWrap,
            bool? isShowOldValue,
            Dictionary<string, bool> sportBetTypeVisibilities = null,
            Dictionary<string, int> resourcesByBetTypeCacheRef = null,
            bool? isShowMyanmarOdds = null,
            Dictionary<string, int> resourcesByProudctCacheRef = null) :
            base(
                currentUserSession,
                dateTimeWrap,
                isShowOldValue,
                sportBetTypeVisibilities,
                resourcesByBetTypeCacheRef,
                isShowMyanmarOdds,
                resourcesByProudctCacheRef)
        {
        }
    }
}