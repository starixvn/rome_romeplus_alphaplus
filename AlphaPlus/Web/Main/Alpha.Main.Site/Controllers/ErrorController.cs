using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authorization;
using AlphaPlus.Core.Filters;

namespace Alpha.Main.Site.Controllers
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