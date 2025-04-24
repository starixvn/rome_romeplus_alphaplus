using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferAllAndClearCredit;

namespace AlphaPlus.Settings.Site.Features.Transfer.Controllers
{
    public class TransferAllAndClearCreditController : TransferAllClearCreditControllerBase
    {
        public TransferAllAndClearCreditController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(CalculationQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(await mediator.Send(message));
        }

        [HttpGet]
        public async Task<ActionResult> TransferResult(CalculationQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View("~/Features/Transfer/Views/TransferAllAndClearCredit/Index.cshtml", await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> TransferResult(TransferResultQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(await mediator.Send(message));
        }
    }
}
