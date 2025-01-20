using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core;
using AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;
using AlphaPlus.Main.Site.Features.ContactUs;


namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardController(
        IMediator mediator,
        IUserSession userSession,
        IOptions<AuthorizationOptions> options)
        : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;
        private readonly AuthorizationOptions options = options.Value;

        [HttpGet]
        [AllowProtectedMode]
        public IActionResult Index()
        {
            if (userSession.IsProtectedMode)
            {
                return View("~/Views/Dashboard/IndexProtectedMode.cshtml");
            }

            if (AppSettings.IsOfferedWhiteLabel(userSession.Site))
            {
                return RedirectToAction("WhiteLabel");
            }

            var userLevelName = Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName;
            if (NovaCasinoSettings.IsOfferedNovaCasino(userSession.Site, userLevelName, userSession.Currency))
            {
                return RedirectToAction("NovaCasino");
            }

            var viewModel = new DashboardBannerViewModel(userSession);

            if (viewModel.IsShow)
            {
                return View("~/Views/Dashboard/Banner.cshtml", viewModel);
            }

            return Index2();
        }

        [HttpGet]
        public IActionResult Index2()
        {
            var viewModel = new DashboardViewModel(userSession.RoleId);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult WhiteLabel()
        {
            var viewModel = new ContactUsViewModel(Request.Host.Value, userSession);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult NovaCasino()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BalanceInfo()
        {
            var viewModel = await mediator.Send(new BalanceInfoQuery(userSession, UserPermission.HasPermissionOnProduct));

            return PartialView(viewModel);
        }

        [HttpGet]
        [AllowProtectedMode]
        public IActionResult GetServerTime(int onUserId = -1)
        {
            if (!options.DisabledCheckDuplicationLogin && ((onUserId > 0 && onUserId != userSession.OnUserId) || userSession.OnUserId == 0))
            {
                return Json(new
                {
                    signOut = true
                });
            }

            var currentTime = DateTime.Now;

            return Json(new
            {
                currentTime.Year,
                currentTime.Month,
                currentTime.Day,
                currentTime.Hour,
                currentTime.Minute,
                currentTime.Second
            });
        }
    }
}