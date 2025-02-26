using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels.Setting;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Setting
{
    public class IndexQuery : LogQuery, IRequest<IndexViewModel>
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
            Dictionary<string, int> resourcesByProductCacheRef = null) :
            base(
                currentUserSession,
                dateTimeWrap,
                isShowOldValue,
                sportBetTypeVisibilities,
                resourcesByBetTypeCacheRef,
                resourcesByProductCacheRef)
        {
        }

        public string TrackingValueTransformTemplatePath { get; set; }
    }
}