namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Transfer.API.AppResources;
    using MediatR;
    using Requests.CashBalanceTransfer;
    using Settings.Transfer.API.Models;
    using TransferResult = Settings.Transfer.API.Models.Result;

    public class CashBalanceTransferController : SiteBaseController
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
            TransferResult result;

            if (command == null || !command.IsValid())
            {
                result = TransferResult.CreateErrorResult(Resources.BadRequest, 400);
            }
            else if (userSession.IsInternal)
            {
                result = TransferResult.CreateErrorResult(Resources.NotAuthorized, 403);
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