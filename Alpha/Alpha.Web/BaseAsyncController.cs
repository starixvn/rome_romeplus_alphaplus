namespace Alpha.Web
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using AlphaPlus.User.Models;
    using Alpha.Web.Helpers;
    using AlphaPlus.Configurations;

    public class BaseAsyncController : AsyncController
    {
        public const string English = "en-US";
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(English);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.EndsWith("/Auth"))
            {
                return;
            }

            base.OnAuthorization(filterContext);

            var isAuthenticated =
                filterContext.HttpContext.User.Identity.IsAuthenticated && UserSession.Current.UserId > 0;

            if (!isAuthenticated)
            {
                UnAuthentication(filterContext);
            }
        }

        protected void UnAuthentication(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write("{ \"message\": \"You are not authorized on this function!\" }");
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                var pathAndQuery = filterContext.HttpContext.Request.Url.PathAndQuery;
                if (!string.IsNullOrWhiteSpace(pathAndQuery))
                {
                    pathAndQuery = $"?ReturnUrl={pathAndQuery}";
                }

                if (ConfigurationManager.GetValue<string>("IsStandaloneReport") == "1")
                {
                    filterContext.Result = new RedirectResult(filterContext.HttpContext.Response.ApplyAppPathModifier($"{FormsAuthentication.LoginUrl}{pathAndQuery}"));
                }
                else
                {
                    filterContext.Result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));
                }
            }
        }

        protected override JsonResult Json(
            object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSession.Current.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = English;
                UserSession.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }
    }
}
