using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Main;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlphaPlus.SecurityCode.Services;
using Microsoft.AspNetCore.Http.Extensions;
using AlphaPlus.Core;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Filters;
using Microsoft.Extensions.Options;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Main.Site.Features.OTP;
using AlphaPlus.Main.Site.Features.Default.MenuBar;
using System.Net;
using AlphaPlus.Main.Site.Features.Dashboard.CashService;

namespace AlphaPlus.Main.Site.Features.Default
{
    public class DefaultController(
        IUserSession userSession,
        IMediator mediator,
        ISecurityCodeChecker securityCodeChecker,
        INicknameChecker nicknameChecker,
        IOptions<AuthorizationOptions> options) : BaseController
    {
        private const string OtpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";
        private readonly IUserSession userSession = userSession;
        private readonly IMediator mediator = mediator;
        private readonly ISecurityCodeChecker securityCodeChecker = securityCodeChecker;
        private readonly INicknameChecker nicknameChecker = nicknameChecker;
        private readonly AuthorizationOptions authorizationOptions = options.Value;

        [AllowProtectedMode]
        public async Task<IActionResult> Index(string action = null)
        {
            if (IsRedirectToNicknamePage(action))
            {
                return RedirectToAction("Index", "Nickname");
            }

            if (await mediator.Send(new CheckDifferenceDeviceCommand(this.userSession)))
            {
                return Redirect(OtpVerifyDifferenceDevicePath);
            }

            var browser = await mediator.Send(new BrowserQuery(userSession));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
            }

            var cashServiceInfoViewModel = await mediator.Send(new GetCashServiceStatusQuery(userSession));

            var query = new IndexQuery(userSession, securityCodeChecker.IsEnableSecurityCode(), null, authorizationOptions);

            var viewModel = await mediator.Send(query);
            viewModel.CashServiceInfoViewModel = cashServiceInfoViewModel;
            
            return View(viewModel);
        }

        [AllowProtectedMode]
        public async Task<IActionResult> MenuBar()
        {
            var viewModel = await mediator.Send(new MenuBarQuery(
                userSession.RoleId,
                Request.Headers.UserAgent.ToString(),
                darkModeName: DarkModeHelper.GetDarkModeName(userSession),
                isHideBanner: AppSettings.BannerIsHide || userSession.IsProtectedMode));

            return PartialView(viewModel);
        }

        [AllowProtectedMode]
        public async Task<IActionResult> KickOut(string reason)
        {
            var message = MapKickOutReasonToUserMessage(reason);

            await mediator.Send(new KickOutQuery(userSession));
            ClearSession();

            return RedirectToAction("Login", "Account", new { r = WebUtility.UrlEncode(message) });
        }

        private void ClearSession()
        {
            AlphaContext.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            AlphaContext.Current.Session.Clear();
        }

        private bool IsRedirectToNicknamePage(string action)
        {
            if (nicknameChecker.IsForceLoginByNickname)
            {
                var displayUrl = Request.GetDisplayUrl();
                return !displayUrl.Contains("nickname", StringComparison.OrdinalIgnoreCase)
                    && !displayUrl.Contains("default", StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrWhiteSpace(action);
            }

            return false;
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

        [AllowProtectedMode]
        public IActionResult Redirect() => View();
    }
}