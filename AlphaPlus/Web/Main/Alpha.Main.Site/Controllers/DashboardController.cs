using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.Requests.Dashboard.BalanceInfo;
using Alpha.Main.Site.ViewModels.Dashboard;
using Microsoft.Extensions.Options;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core;


namespace Alpha.Main.Site.Controllers
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

            return Index2();
        }

        [HttpGet]
        public IActionResult Index2()
        {
            var viewModel = new DashboardViewModel(userSession.RoleId);

            return View(viewModel);
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
            if (options.DisabledCheckDuplicationLogin && ((onUserId > 0 && onUserId != userSession.OnUserId) || userSession.OnUserId == 0))
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