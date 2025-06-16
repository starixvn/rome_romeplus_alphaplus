using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using UserLevel = AlphaPlus.Features.Messages.API.Models.Constants.UserLevel;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;
using Microsoft.AspNetCore.OutputCaching;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
    [AllowProtectedMode]
    public class StaticMessageController : BaseController
    {
        private readonly IDictionary<string, string> FileNameMapping = new Dictionary<string, string>
        {
            ["167819"] = "TelegramChatbot",
            ["167820"] = "TelegramChatbot",
            ["170467"] = "MyWatchlist"
        };

        private readonly ResourceManager resourceManager;

        private readonly IUserSession userSession;

        private readonly string baseDirectory;

        private readonly IRazorViewEngine razorViewEngine;

        public StaticMessageController(ResourceManager resourceManager, IUserSession userSession, IRazorViewEngine razorViewEngine)
        {
            this.resourceManager = resourceManager;
            this.userSession = userSession;
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.razorViewEngine = razorViewEngine;
        }

        public ActionResult NewSportType()
            => View();

        public ActionResult Index(string id)
            => View(id);

        [ResponseCache(NoStore = true), OutputCache(NoStore = true)]
        public ActionResult File(string id)
        {
            string viewName = $"{id}.{userSession.Language}";
            if (!razorViewEngine.GetView(baseDirectory, $"~/Views/StaticMessage/{viewName}.cshtml",  false).Success)
            {
                viewName = $"{id}.en-US";
            }

            if (!razorViewEngine.GetView(baseDirectory, $"~/Views/StaticMessage/{viewName}.cshtml", false).Success)
            {
                if (!razorViewEngine.GetView(baseDirectory, $"~/Views/StaticMessage/{id}.cshtml", false).Success)
                {
                    return new EmptyResult();
                }

                return PartialView(id);
            }

            return PartialView(viewName);
        }

        [ResponseCache(NoStore = true), OutputCache(NoStore = true)]
        public ActionResult Guide(string id)
        {
            var roleName = UserLevel.GetDisplayName(userSession.RoleId);
            string fileName = $"{id}.{roleName}.{userSession.Language}";
            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                fileName = $"{id}.{roleName}.en-US";
            }

            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                return new EmptyResult();
            }

            var fileDownloadName = FileNameMapping.ContainsKey(id)
                ? $"{resourceManager.GetString(FileNameMapping[id], CultureInfo.CreateSpecificCulture(userSession.Language))}.pdf"
                : "Guide.pdf";

            Response.Headers.Append("Content-Disposition", "inline; filename=" + WebUtility.UrlEncode(fileDownloadName));

            return base.PhysicalFile($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }

        [ResponseCache(NoStore = true), OutputCache(NoStore = true)]
        public ActionResult Help(string id)
        {
            string fileName = $"{id}.{userSession.Language}";
            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                fileName = $"{id}.en-US";
            }

            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                return new EmptyResult();
            }

            var fileDownloadName = FileNameMapping.ContainsKey(id)
                ? $"{resourceManager.GetString(FileNameMapping[id], CultureInfo.CreateSpecificCulture(userSession.Language))}.pdf"
                : "Guide.pdf";

            Response.Headers.Append("Content-Disposition", "inline; filename=" + WebUtility.UrlEncode(fileDownloadName));

            return base.PhysicalFile($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }
    }
}