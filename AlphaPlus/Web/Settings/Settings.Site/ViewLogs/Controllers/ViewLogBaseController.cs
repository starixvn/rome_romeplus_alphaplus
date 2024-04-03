namespace AlphaPlus.Settings.Site.ViewLogs.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Core.Models;
    using Configurations;
    using Helpers;
    using AlphaPlus.Core;

    public abstract class ViewLogBaseController : BaseController
    {
        private readonly MicroSites microSites;

        protected ViewLogBaseController(IUserSession userSession, MicroSites microSites)
        {
            UserSession = userSession;
            this.microSites = microSites;
        }

        public IUserSession UserSession { get; set; }

        // protected override  void OnAuthorization(AuthorizationContext filterContext)
        // {
        //     base.OnAuthorization(filterContext);

        //     if (UserSession.IsSubAcc && !UserSession.SubPageAccess.ViewLog)
        //     {
        //         Response.Redirect(microSites.OldMainPath(AppSettings.AccessDeniedPage));
        //     }
        // }
    }
}