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
        public ActionResult Index(CalculationQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }

        [HttpGet]
        public ActionResult TransferResult(CalculationQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View("~/Transfer/Views/TransferAllAndClearCredit/Index.cshtml", mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult TransferResult(TransferResultQuery message)
        {
            if (message == null || !message.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }
    }
}
