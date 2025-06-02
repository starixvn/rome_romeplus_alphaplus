using AlphaPlus.Core;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.Language
{
    public class LanguageController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IUserSession userSession = userSession;
        private readonly IMediator mediator = mediator;

        [HttpPost]
        [AllowProtectedMode]
        public async Task<JsonResult> Change(string languageCode, bool? displayLanguage)
        {
            if (displayLanguage.HasValue && displayLanguage.Value && userSession.IsInternal)
            {
                return Json(null);
            }

            var viewModel = await mediator.Send(
                new ChangeLanguageCommand(userSession, languageCode, AppSettings.DisabledVietnamese, displayLanguage ?? false));

            var domain = Request.Headers.Host.ToString();
            var subDomain = domain.Split('.')[0];
            var mainDomain = domain.Replace(subDomain, string.Empty);
            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false };
            Response.Cookies.Append("LANGUAGE", viewModel, cookieOptions);
            Response.Cookies.Append("hidLanguage", viewModel, cookieOptions);

            return Json(viewModel);
        }
    }
}