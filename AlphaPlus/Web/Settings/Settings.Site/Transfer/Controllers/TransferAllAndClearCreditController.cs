namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Requests.TransferAllAndClearCredit;
    
    
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

            return View("~/Transfer/Views/TransferAllAndClearCredit/Index.cshtml", await mediator.Send(message));
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
