using System.Security.Claims;
using System.Text.Json;
using AlphaPlus.Shared.AppResources.Main;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.Core.Helpers;
using System.Net;

namespace AlphaPlus.Main.Site.Features.Account;

[AllowAnonymous]
public class AccountController(
    IUserSession userSession,
    IDeviceHelper deviceHelper,
    IAccountService accountService,
    IAesService aesService,
    IOptions<Core.Configurations.AuthorizationOptions> options)
    : Controller
{
    private readonly IUserSession userSession = userSession;
    private readonly IAccountService accountService = accountService;
    private readonly IAesService aesService = aesService;
    private readonly IDeviceHelper deviceHelper = deviceHelper;
    private readonly Core.Configurations.AuthorizationOptions options = options.Value;

    private const string SecretKey = "!@#$";

    public IActionResult Login(string r = null)
    {
        var domains = DomainsHelper.DomainMappings;
        var loginUrl = domains.FirstOrDefault(domain => domain.After.Contains(Request.Headers["Host"].ToString(), StringComparison.OrdinalIgnoreCase));
        var loginResultMessage = $"?r={WebUtility.UrlEncode(string.Equals(r, "SessionExpired", StringComparison.OrdinalIgnoreCase) ? 
            Resources.SessionExpired : r ?? string.Empty)}";

        IActionResult result = userSession.IsIndirectLoggedIn || userSession.IsInternal || loginUrl == null || string.IsNullOrWhiteSpace(loginUrl.Before) ?
            new EmptyResult() : Redirect($"{Request.Scheme}://{loginUrl.Before}{loginResultMessage}");

        ClearSessionCookies();

        return result;
    }

    [Authorize]
    public async Task<IActionResult> Logout(string r = null)
    {
        if (!options.DisabledCheckDuplicationLogin)
        {
            await accountService.CheckOut();
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (userSession.IsIndirectLoggedIn || userSession.IsInternal)
        {
            ClearSessionCookies();

            return new EmptyResult();
        }

        return Login(r);
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

    private void ClearSessionCookies()
    {
        AlphaContext.Current.Session.Clear();

        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }
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
        requestInfo.BrowserName = !string.IsNullOrWhiteSpace(param.BrowserSize) ?
            $"{requestInfo.BrowserName}, {param.BrowserSize}" : requestInfo.BrowserName;

        var signInResult = await accountService.AfterSignIn(new SignInParam
        {
            CustId = param.CustId,
            Password = aesService.Decrypt(param.EncryptPassword, param.EncryptCode),
            SubAccId = param.SubAccId,
            Nickname = param.Nickname,
            IsNickname = param.IsNickname,
            LoginTime = new DateTime(param.LoginTime),
            Language = userSession.LangKey,
            RequestInfo = requestInfo,
            DarkModeName = param.DarkModeName,
            AssetAppPath = param.AssetAppPath,
            DCSStaticAppPath = param.DCSStaticAppPath
        });

        if (signInResult.IsSuccessful)
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
            return RedirectToAction("Redirect", "Default");
        }

        return Login(signInResult.Message);
    }

    [HttpGet]
    public async Task<IActionResult> InternalLogin(string parameter)
    {
        var reqParam = aesService.Decrypt(parameter, SecretKey);
        var internalLoginParams = reqParam.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        var requestInfo = await deviceHelper.GetRequestInfo();
        var param = new InternalLoginParam
        {
            Username = internalLoginParams.ElementAt(0),
            AdminId = int.Parse(internalLoginParams.ElementAt(1)),
            LoginTime = new DateTime(long.Parse(internalLoginParams.ElementAt(2))),
            RequestInfo = requestInfo
        };

        var signInResult = await accountService.InternalLogin(param);

        if (signInResult.IsSuccessful)
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
            return RedirectToAction("Redirect", "Default");
        }

        return new EmptyResult();
    }

    [HttpGet]
    public async Task<IActionResult> IndirectLogin(string parameter)
    {
        var reqParam = aesService.Decrypt(parameter, SecretKey);
        var indirectLoginParams = reqParam.Split(['&']);
        var requestInfo = await deviceHelper.GetRequestInfo();
        var param = new IndirectLoginParam
        {
            CustId = int.Parse(indirectLoginParams.ElementAt(0)),
            SubAccId = int.Parse(indirectLoginParams.ElementAt(1)),
            LoginTime = new DateTime(long.Parse(indirectLoginParams.ElementAt(2))),
            Language = indirectLoginParams.ElementAt(3),
            UplineCustId = int.Parse(indirectLoginParams.ElementAt(4)),
            UplineSubAccId = int.Parse(indirectLoginParams.ElementAt(5)),
            UplineUsername = indirectLoginParams.ElementAt(6),
            Username = indirectLoginParams.ElementAt(7),
            DarkModeName = indirectLoginParams.ElementAt(8),
            AssetAppPath = indirectLoginParams.ElementAt(9),
            RequestInfo = requestInfo
        };

        var signInResult = await accountService.IndirectLogin(param);

        if (signInResult.IsSuccessful)
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
            return RedirectToAction("Redirect", "Default");
        }

        return new EmptyResult();
    }
}