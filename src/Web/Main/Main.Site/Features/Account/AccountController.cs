using System.Globalization;
using System.Net;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.RequestInfo;
using AlphaPlus.Shared.AppResources.Main;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Main.Site.Features.Account;

[AllowAnonymous]
public class AccountController(
    IUserSession userSession,
    IDeviceHelper deviceHelper,
    IAccountService accountService,
    IOptions<Core.Configurations.AuthorizationOptions> options)
    : Controller
{
    private readonly IUserSession userSession = userSession;
    private readonly IAccountService accountService = accountService;
    private readonly IDeviceHelper deviceHelper = deviceHelper;
    private readonly Core.Configurations.AuthorizationOptions options = options.Value;

    [AllowProtectedMode]
    public IActionResult Login(string r = null)
    {
        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        var scheme = isDev ? "http" : "https";
        var domains = DomainsHelper.DomainMappings;
        var loginUrl = domains.FirstOrDefault(domain => domain.After.Contains(Request.Headers["Host"].ToString(), StringComparison.OrdinalIgnoreCase));
        var loginResultMessage = $"{WebUtility.UrlEncode(string.Equals(r, "SessionExpired", StringComparison.OrdinalIgnoreCase) ?
            Resources.ResourceManager.GetString("SessionExpired", new CultureInfo(Request.Cookies["LANGUAGE"] ?? "en-US")) : r ?? string.Empty)}";

        loginResultMessage = string.IsNullOrWhiteSpace(loginResultMessage) ? string.Empty : $"?r={loginResultMessage}";

        IActionResult result = userSession.IsIndirectLoggedIn || userSession.IsInternal || loginUrl == null || string.IsNullOrWhiteSpace(loginUrl.Before) ?
            new EmptyResult() : Redirect($"{scheme}://{loginUrl.Before}{loginResultMessage}");

        return result;
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

    private static void ClearSession()
    {
        AlphaContext.Current.Session.Clear();
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
}