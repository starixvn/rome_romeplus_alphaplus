namespace AlphaPlus.Settings.Site.Features.ViewLogs.Controllers
{
    using AlphaPlus.Core.Models;
    using Configurations;
    using Helpers;
    using AlphaPlus.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    public abstract class ViewLogBaseController : BaseController
    {
        private readonly MicroSites microSites;

        protected ViewLogBaseController(IUserSession userSession, MicroSites microSites)
        {
            UserSession = userSession;
            this.microSites = microSites;
        }

        public IUserSession UserSession { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (UserSession.IsSubAcc && !UserSession.SubPageAccess.ViewLog)
            {
                Response.Redirect(microSites.OldMainPath(AppSettings.AccessDeniedPage));
            }
        }
    }
}