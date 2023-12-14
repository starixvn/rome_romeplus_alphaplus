using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using Alpha.Main.AppResources;
using Alpha.Main.Configurations;
using MediatR;
using Alpha.Main.Site.Models.Requests;
using Alpha.Main.Site.Models.Requests.UserMessages;
using Alpha.Main.Site.Requests;
using Alpha.Main.Site.Requests.Otp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlphaPlus.SecurityCode.Services;
using Microsoft.AspNetCore.Http.Extensions;
using AlphaPlus.Core;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Filters;


namespace Alpha.Main.Site.Controllers
{
    public class DefaultController(
        IUserSession userSession,
        IMediator mediator,
        ISecurityCodeChecker securityCodeChecker,
        INicknameChecker nicknameChecker) : BaseController
    {
        private const string OtpVerifyDifferenceDevicePath = "~/otp/VerifyOtpDifferenceDevice";
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";
        private readonly IUserSession userSession = userSession;
        private readonly IMediator mediator = mediator;
        private readonly ISecurityCodeChecker securityCodeChecker = securityCodeChecker;
        private readonly INicknameChecker nicknameChecker = nicknameChecker;

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

            var query = new IndexQuery(userSession, securityCodeChecker.IsEnableSecurityCode(), null, nicknameChecker);

            var viewModel = await mediator.Send(query);

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
        public async Task<IActionResult> HeaderHotMsg()
        {
            var query = new HotMessagesQuery
            {
                Language = userSession.Language,
                RoleId = userSession.RoleId
            };

            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [AllowProtectedMode]
        public async Task<IActionResult> KickOut(string reason)
        {
            var message = MapKickOutReasonToUserMessage(reason);

            await mediator.Send(new KickOutQuery(userSession));
            ClearSession();

            return View(message as object);
        }

        // TODO: DisabledVietnamese
        // protected override void Initialize(RequestContext requestContext)
        // {
        //     var cultureName = UserSession.Current.Language;

        //     if (AppSettings.DisabledVietnamese && cultureName == Language.Vietnamese)
        //     {
        //         cultureName = English;
        //         UserSession.Current.Language = cultureName;
        //     }

        //     base.Initialize(requestContext);
        // }

        private static void ClearSession()
        {
            AlphaContext.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private bool IsRedirectToNicknamePage(string action)
        {
            if (nicknameChecker.IsRecommendLoginByNickname)
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
    }
}