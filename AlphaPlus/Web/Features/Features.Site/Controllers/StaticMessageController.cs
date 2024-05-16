namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Features.Messages.API.Models.Announcement;
    using AlphaPlus.Core.Filters;
    using AlphaPlus.Core;
    using AlphaPlus.Core.Models;
    using UserLevel = Features.Messages.API.Models.Constants.UserLevel;

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

        public StaticMessageController(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public ActionResult NewSportType()
            => View();

        public ActionResult Index(string id)
            => View(id);

        public ActionResult File(string id)
        {
            string viewName = $"{id}.{UserSession.Current.Language}";
            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{viewName}.cshtml"))
            {
                viewName = $"{id}.en-US";
            }

            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{viewName}.cshtml"))
            {
                return new EmptyResult();
            }

            return PartialView(viewName);
        }

        public ActionResult Guide(string id)
        {
            var roleName = UserLevel.GetDisplayName(UserSession.Current.RoleId);
            string fileName = $"{id}.{roleName}.{UserSession.Current.Language}";
            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                fileName = $"{id}.{roleName}.en-US";
            }

            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                return new EmptyResult();
            }

            var fileDownloadName = FileNameMapping.ContainsKey(id) 
                ? $"{resourceManager.GetString(FileNameMapping[id], CultureInfo.CreateSpecificCulture(UserSession.Current.Language))}.pdf" 
                : "Guide.pdf";

            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileDownloadName);

            return base.PhysicalFile($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }

        public ActionResult Help(string id)
        {
            string fileName = $"{id}.{UserSession.Current.Language}";
            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                fileName = $"{id}.en-US";
            }

            if (!System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf"))
            {
                return new EmptyResult();
            }

            var fileDownloadName = FileNameMapping.ContainsKey(id)
                ? $"{resourceManager.GetString(FileNameMapping[id], CultureInfo.CreateSpecificCulture(UserSession.Current.Language))}.pdf"
                : "Guide.pdf";

            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileDownloadName);

            return base.PhysicalFile($"{AppDomain.CurrentDomain.BaseDirectory}/Views/StaticMessage/{fileName}.pdf", "application/pdf");
        }
    }
}