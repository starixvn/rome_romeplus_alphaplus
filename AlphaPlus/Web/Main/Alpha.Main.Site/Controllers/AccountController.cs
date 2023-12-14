using Alpha.Main.Site.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Main.Site.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginInputViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputViewModel model)
    {
        return Ok();
    }

    // [HttpGet]
    // public async Task<IActionResult> SSO(string token)
    // {
    //     var parameters = token.Replace(' ', '+');

    // }
}
