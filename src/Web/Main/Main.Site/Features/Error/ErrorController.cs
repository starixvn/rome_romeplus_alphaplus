using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authorization;
using AlphaPlus.Core.Filters;

namespace AlphaPlus.Main.Site.Features.Error
{
    [AllowAnonymous]
    [AllowProtectedMode]
    public class ErrorController(IUserSession userSession) : Controller
    {
        private readonly IUserSession userSession = userSession;

        public IActionResult Index()
            => View((object)DarkModeHelper.GetDarkModeName(userSession));

        public IActionResult Blocked()
            => View((object)DarkModeHelper.GetDarkModeName(userSession));
    }
}