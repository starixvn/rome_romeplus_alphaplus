using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Messages.API.Models.Announcement;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using UserLevel = AlphaPlus.Features.Messages.API.Models.Constants.UserLevel;

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

        public StaticMessageController(ResourceManager resourceManager, IUserSession userSession)
        {
            this.resourceManager = resourceManager;
            this.userSession = userSession;
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public ActionResult NewSportType()
            => View();

        public ActionResult Index(string id)
            => View(id);

        public ActionResult File(string id)
        {
            string viewName = $"{id}.{userSession.Language}";
            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{viewName}.cshtml"))
            {
                viewName = $"{id}.en-US";
            }

            if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{viewName}.cshtml"))
            {
                if (!System.IO.File.Exists($"{baseDirectory}/Views/StaticMessage/{id}.cshtml"))
                {
                    return new EmptyResult();
                }

                return PartialView(id);
            }

            return PartialView(viewName);
        }

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

            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileDownloadName);

            return base.PhysicalFile($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }

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

            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileDownloadName);

            return base.PhysicalFile($"{baseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }
    }
}