namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Shared.AppResources.Settings.Transfer;
    using AlphaPlus.Settings.Transfer.API.Configurations;
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using AlphaPlus.Settings.Transfer.API.Services;
    using AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer;
    using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer;
    using AlphaPlus.Core.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AutoTransferController : TransferBaseController
    {
        private readonly IUserSession userSession;
        private readonly TransferService transferService;
        private const string InvalidTransferStatusViewPath = "~/Features/Transfer/Views/AutoTransfer/ADBTChange.cshtml";
        private const int ADBTChangeCode = 108;

        public AutoTransferController(
            IMediator mediator,
            IUserSession userSession,
            TransferService transferService)
            : base(mediator)
        {
            this.userSession = userSession;
            this.transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSingleAutoTransfer(UpdateQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return await View(message);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.Send(message);
            }

            return Json(result);
        }

        [HttpGet]
        public ViewResult Monitoring(MonitoringQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(new MonitoringViewModel(userSession, message));
        }

        [HttpGet]
        public ViewResult History()
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(new HistoryViewModel(userSession));
        }

        [HttpGet]
        public ViewResult Hint()
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetTodayTransfers(GetTodayTransfersQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return Json(Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode));
            }

            var result = await mediator.Send(message);

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoryTransfers(GetHistoryTransfersQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return Json(Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode));
            }

            var result = await mediator.Send(message);

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> CancelJob(CancelJobQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.Send(message);
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> CancelAllJobs(CancelAllJobsQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.Send(message);
            }

            return Json(result);
        }

        [HttpGet]
        public ViewResult UpdateSchedule(UpdateScheduleQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(message);
        }

        [HttpPost]
        public async Task<ActionResult> PostUpdateSchedule(PostUpdateScheduleQuery message)
        {
            Result result;

            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                result = Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else if (message == null || !message.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else
            {
                result = await mediator.Send(message);
            }

            return Json(result);
        }

        [HttpGet]
        public ViewResult UpdateMultipleAutoTransfer(UpdateMultipleAutoTransferQuery message)
        {
            if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
            {
                return View(InvalidTransferStatusViewPath);
            }

            return View(message);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMultipleAutoTransfer(PostUpdateMultipleAutoTransferQuery message)
        {
            UpdateMultipleAutoTransferResultModel result;

            if (userSession.IsInternal)
            {
                result = new UpdateMultipleAutoTransferResultModel(403, Resources.NotAuthorized);
            }
            else if (message == null)
            {
                result = new UpdateMultipleAutoTransferResultModel(400, Resources.BadRequest);
            }
            else
            {
                result = await mediator.Send(message);
            }

            return Json(result);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //var method = filterContext.HttpContext?.Request?.Method;

            if (AppSettings.CheckOfferAutoTransferCurrencies(UserSession.Current.Currency))
            {
                return;
            }

            filterContext.Result = new UnauthorizedObjectResult("You are not authorized on this function!");
            filterContext.HttpContext.Session.Clear();
        }
    }
}