using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers
{
    public class CashBalanceTransferController : TransferBaseController
    {
        private readonly IUserSession userSession;

        public CashBalanceTransferController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpGet]
        public async Task<ActionResult> CashBalance(CashBalanceTransferQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CashBalance(CashBalanceTransferCommand command)
        {
            Result result;

            if (command == null || !command.IsValid())
            {
                result = Result.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
            else
            {
                result = await mediator.Send(command);
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Hint(HintTransferQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            var viewModel = await mediator.Send(message);

            return View(GetHintViewNameByLanguage(message.Language), viewModel);
        }

        [NonAction]
        private string GetHintViewNameByLanguage(string language)
        {
            switch (language)
            {
                case "zh-TW":
                case "zh-CN":
                case "th-TH":
                case "vi-VN":
                    return $"{nameof(Hint)}.{language}";

                default:
                    return nameof(Hint);
            }
        }
    }
}