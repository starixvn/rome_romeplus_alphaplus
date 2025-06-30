using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Credit
{
    public class CreditLogExcelQuery : LogQuery, IRequest<ExcelFileViewModel>
    {
        public CreditLogExcelQuery() : this(null, null, null)
        {
        }

        public CreditLogExcelQuery(
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
    }
} 