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

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers;

public class AutoTransferController(
    IMediator mediator,
    IUserSession userSession,
    ITransferService transferService) : TransferBaseController(mediator)
{
    private const int ADBTChangeCode = 108;
    private const string InvalidTransferStatusViewPath = "~/Features/Transfer/Views/AutoTransfer/ADBTChange.cshtml";
    private readonly IUserSession userSession = userSession;
    private readonly ITransferService transferService = transferService;

    [HttpGet]
    public async Task<IActionResult> UpdateSingleAutoTransfer(UpdateQuery message)
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View((object)message);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateQuery message)
    {
        Result result;

        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
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
    public async Task<IActionResult> Monitoring(MonitoringQuery message)
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View(new MonitoringViewModel(userSession, message));
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View(new HistoryViewModel(userSession));
    }

    [HttpGet]
    public async Task<IActionResult> Hint()
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetTodayTransfers(GetTodayTransfersQuery message)
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return Json(Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode));
        }

        var result = await mediator.Send(message);

        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetHistoryTransfers(GetHistoryTransfersQuery message)
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return Json(Result.CreateErrorResult(Resources.ADBTChangeError, ADBTChangeCode));
        }

        var result = await mediator.Send(message);

        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> CancelJob(CancelJobQuery message)
    {
        Result result;

        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
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
    public async Task<IActionResult> CancelAllJobs(CancelAllJobsQuery message)
    {
        Result result;

        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
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
    public async Task<IActionResult> UpdateSchedule(UpdateScheduleQuery message)
    {
        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View(message);
    }

    [HttpPost]
    public async Task<ActionResult> PostUpdateSchedule(PostUpdateScheduleQuery message)
    {
        Result result;

        if (await transferService.IsAllDayBalanceTransferAsync(userSession.CustId))
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
    public IActionResult UpdateMultipleAutoTransfer(UpdateMultipleAutoTransferQuery message)
    {
        if (transferService.IsAllDayBalanceTransfer(userSession.CustId))
        {
            return View(InvalidTransferStatusViewPath);
        }

        return View(message);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMultipleAutoTransfer(PostUpdateMultipleAutoTransferQuery message)
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