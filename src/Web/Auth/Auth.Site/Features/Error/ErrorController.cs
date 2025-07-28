using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authorization;
using AlphaPlus.Core.Filters;
using AlphaPlus.NetThrottling;
using AlphaPlus.Auth.Site.Helpers;

namespace AlphaPlus.Auth.Site.Features.Error;

[AllowAnonymous]
[AllowProtectedMode]
[DisableNetThrottling]
public class ErrorController(IUserSession userSession) : Controller
{
     public IActionResult Index()
            => View((object)DarkModeHelper.GetDarkModeName(userSession));

    public IActionResult Blocked()
        => View((object)DarkModeHelper.GetDarkModeName(userSession));
}