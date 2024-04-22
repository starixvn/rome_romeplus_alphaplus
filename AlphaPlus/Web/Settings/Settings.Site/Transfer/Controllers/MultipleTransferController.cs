namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Requests.MultipleTransfer;

    public class MultipleTransferController : SiteBaseController
    {
        public MultipleTransferController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> CustomerList(CustomerListQuery message)
            => View(await mediator.Send(message));
    }
}