using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Shared
{
    [SiteAuthorizationFilter]
    public class SiteBaseController : BaseController
    {
        protected readonly IUserSession _userSession;

        protected SiteBaseController(IUserSession userSession)
        {
            _userSession = userSession;
        }
    }
}