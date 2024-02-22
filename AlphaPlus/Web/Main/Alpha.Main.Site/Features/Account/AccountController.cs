using System.Security.Claims;
using System.Text.Json;
using Alpha.Main.Site.AppResources;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Alpha.Main.Site.Features.Account;

[AllowAnonymous]
public class AccountController(
    IUserSession userSession,
    IConfiguration configuration,
    IDeviceHelper deviceHelper,
    IAccountService accountService,
    IAesService aesService,
    IOptions<AlphaPlus.Core.Configurations.AuthorizationOptions> options)
    : Controller
{
    private readonly IUserSession userSession = userSession;
    private readonly IConfiguration configuration = configuration;
    private readonly IAccountService accountService = accountService;
    private readonly IAesService aesService = aesService;
    private readonly IDeviceHelper deviceHelper = deviceHelper;
    private readonly AlphaPlus.Core.Configurations.AuthorizationOptions options = options.Value;

    private const string SecretKey = "!@#$";

    public IActionResult Login(string returnUrl)
    {
        // TODO: mapping before domain
        var beforeUrl = configuration.GetValue<string>("BeforeUrl");
        return Redirect(beforeUrl);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        if (!options.DisabledCheckDuplicationLogin)
        {
            await accountService.CheckOut();
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var beforeUrl = Url.Action(nameof(Login), "Account");
        return SignOut(new AuthenticationProperties { RedirectUri = beforeUrl });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ReCheckIn(int onid)
    {
        var result = onid < 0 ? new ReCheckInResult { IsKickedOut = true } : await accountService.ReCheckIn(onid);
        return Json(result.IsKickedOut
            ? Result.CreateErrorResult(MapKickOutReasonToUserMessage(result.KickOutReason))
            : Result.CreateSuccessfulResult());
    }

    private static string MapKickOutReasonToUserMessage(byte kickOutReason)
    {
        const byte ResetPasswordForceKickOut = 1;
        const byte ProtectedModeForceKickOut = 2;

        return kickOutReason switch
        {
            ResetPasswordForceKickOut => Security.KickoutMessageByForceChangePassword,
            ProtectedModeForceKickOut => Security.KickoutMessageByProtectedMode,
            _ => Security.stringSessionLogged.Replace("\n", string.Empty),
        };
    }


    [HttpGet]
    public async Task<IActionResult> SSO(string parameter)
    {
        var param = JsonSerializer.Deserialize<SSOParam>(aesService.Decrypt(parameter, SecretKey));
        var requestInfo = await deviceHelper.GetRequestInfo();
        var isSignedIn = await accountService.AfterSignIn(new SignInParam
        {
            CustId = param.CustId,
            Password = aesService.Decrypt(param.EncryptPassword, param.EncryptCode),
            SubAccId = param.SubAccId,
            Nickname = param.Nickname,
            IsNickname = param.IsNickname,
            LoginTime = new DateTime(param.LoginTime),
            Language = userSession.LangKey,
            RequestInfo = requestInfo
        });

        if (isSignedIn)
        {
            if (!options.DisabledCheckDuplicationLogin)
            {
                await accountService.CheckIn();
            }

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


