using System.Text.Json;
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

    [HttpGet]
    public async Task<IActionResult> SSO(string parameter)
    {
        var param = JsonSerializer.Deserialize<SSOParam>(parameter);
        return Ok();
    }
}

public class SSOParam
{
    public int CustId { get; set; }

    public int SubAccId { get; set; }

    public long LoginTime { get; set; }

    public string EncryptPassword { get; set; }

    public string Nickname { get; set; }

    public bool IsNickname { get; set; }

    public string Language { get; set; }

    public bool NumberCrossCookie { get; set; }

    public string DeviceId { get; set; }

    public string BrowserSize { get; set; }

    public string EncryptCode { get; set; }

    public string DarkModeName { get; set; }

    public string AssetAppPath { get; set; }

    public string UMToken { get; set; }
}
