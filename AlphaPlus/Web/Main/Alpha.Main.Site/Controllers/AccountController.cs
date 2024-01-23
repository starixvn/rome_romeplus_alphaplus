using System.Security.Claims;
using System.Text.Json;
using Alpha.Main.Params.Authorization;
using Alpha.Main.Services;
using Alpha.Main.Site.ViewModels.Account;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Main.Site.Controllers;

[AllowAnonymous]
public class AccountController(IDeviceHelper deviceHelper, ISignInService signInService, IAesService aesService) : Controller
{
    private readonly ISignInService signInService = signInService;
    private readonly IAesService aesService = aesService;
    private readonly IDeviceHelper deviceHelper = deviceHelper;

    private const string SecretKey = "!@#$";

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
        var param = JsonSerializer.Deserialize<SSOParam>(aesService.Decrypt(parameter, SecretKey));
        var requestInfo = await deviceHelper.GetRequestInfo();
        var isSignedIn = await signInService.AfterSignIn(new SignInParam
        {
            CustId = param.CustId,
            Password = aesService.Decrypt(param.EncryptPassword, param.EncryptCode),
            SubAccId = param.SubAccId,
            Nickname = param.Nickname,
            IsNickname = param.IsNickname,
            LoginTime = new DateTime(param.LoginTime),
            Language = UserSession.Current.LangKey,
            RequestInfo = requestInfo
        });

        if (isSignedIn)
        {
            var claims = new List<Claim>{
                new (ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Default");
        }

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
