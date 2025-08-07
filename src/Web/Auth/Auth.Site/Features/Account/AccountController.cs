using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Auth.Site.Helpers;
using AlphaPlus.RequestInfo;
using AlphaPlus.Shared.AppResources.Main;
using Fanex.Cryptography.Aes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Auth.Site.Features.Account;

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

    private string DefaultAfterLoggedInUrl => $"{Request.Scheme}://{Request.Host}/main/Default/Redirect";

    [AllowAnonymous, HttpGet, AllowProtectedMode]
    public IActionResult Login(string r = null)
    {
        var domains = DomainsHelper.DomainMappings;
        var loginUrl = domains.FirstOrDefault(domain => domain.After.Contains(Request.Headers["Host"].ToString(), StringComparison.OrdinalIgnoreCase));
        var loginResultMessage = $"{WebUtility.UrlEncode(string.Equals(r, "SessionExpired", StringComparison.OrdinalIgnoreCase) ?
            Resources.ResourceManager.GetString("SessionExpired", new CultureInfo(Request.Cookies["LANGUAGE"] ?? "en-US")) : r ?? string.Empty)}";

        loginResultMessage = string.IsNullOrWhiteSpace(loginResultMessage) ? string.Empty : $"?r={loginResultMessage}";

        IActionResult result = userSession.IsIndirectLoggedIn || userSession.IsInternal || loginUrl == null || string.IsNullOrWhiteSpace(loginUrl.Before) ?
            new EmptyResult() : Redirect($"{Request.Scheme}://{loginUrl.Before}{loginResultMessage}");

        return result;
    }

    private static void ClearSession()
    {
        AlphaContext.Current.Session.Clear();
    }

    [AllowAnonymous, HttpGet, AllowProtectedMode]
    public async Task<IActionResult> SSO(string parameter)
    {
        var invalidSession = WebUtility.UrlEncode(Resources.ResourceManager.GetString("invalidsession", new CultureInfo(Request.Cookies["LANGUAGE"] ?? "en-US")));
        if (string.IsNullOrWhiteSpace(parameter))
        {
            return Login(invalidSession);
        }

        SSOParam param;
        try
        {
            param = JsonSerializer.Deserialize<SSOParam>(aesService.Decrypt(parameter, SecretKey));
        }
        catch
        {
            return Login(invalidSession);
        }

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
            
            return Redirect(DefaultAfterLoggedInUrl);
        }

        return Login(WebUtility.UrlEncode(signInResult.Message));
    }

    [AllowAnonymous, HttpGet, AllowProtectedMode]
    public async Task<IActionResult> InternalLogin(string parameter)
    {
        var reqParam = aesService.Decrypt(parameter, SecretKey);

        if (string.IsNullOrWhiteSpace(parameter) || string.IsNullOrWhiteSpace(reqParam))
        {
            return new EmptyResult();
        }

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
            
            return Redirect(DefaultAfterLoggedInUrl);
        }

        return new EmptyResult();
    }

    [AllowAnonymous, HttpGet, AllowProtectedMode]
    public async Task<IActionResult> IndirectLogin(string parameter)
    {
        ClearSession();

        var reqParam = aesService.Decrypt(parameter, SecretKey);

        if (string.IsNullOrWhiteSpace(parameter) || string.IsNullOrWhiteSpace(reqParam))
        {
            return new EmptyResult();
        }

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

            return Redirect(DefaultAfterLoggedInUrl);
        }

        return new EmptyResult();
    }

    [AllowProtectedMode]
    public IActionResult Logout(string r = null)
    {
        return Login(r);
    }

    [Authorize, AllowProtectedMode]
    public async Task<IActionResult> Logout2()
    {
        if (!options.DisabledCheckDuplicationLogin)
        {
            try
            {
                await accountService.CheckOut();
            }
            finally
            {
                // Prevent exception from sql login account is disabled
            }
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (userSession.IsIndirectLoggedIn || userSession.IsInternal)
        {
            ClearSession();

            return new EmptyResult();
        }

        ClearSession();

        return Login();
    }

    [HttpPost]
    [Authorize, AllowProtectedMode]
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

    [HttpGet, AllowProtectedMode]
    public async Task<IActionResult> Offline()
    {
        return await Logout2();
    }

    [Authorize, AllowProtectedMode]
    public async Task<IActionResult> KickOut(string reason)
    {
        var message = MapKickOutReasonToUserMessage(reason);

        await accountService.CheckOut(" Kicked out: Session Changed");
        ClearSession();

        return Login(WebUtility.UrlEncode(message));
    }

    private static string MapKickOutReasonToUserMessage(string reason)
    {
        return reason switch
        {
            "DuplicationLogin" => Security.stringSessionLogged,
            "AccountClosed" => Security.lblConfirmClosed,
            "AccountLocked" => Security.lblConfirmLocked,
            "KickoutByForceChangePassword" => Security.KickoutMessageByForceChangePassword,
            "KickoutByProtectedMode" => Security.KickoutMessageByProtectedMode,
            _ => Resources.NoPermissionAccessPage,
        };
    }
}