using AlphaPlus.CashService.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Transactions
{
    public class TransactionsController(IMediator mediator) : SiteBaseController(mediator)
    {
        public IActionResult Index()
        {
            return Ok("Transactions");
        }

        public IActionResult Deposit()
        {
            return View();
        }
    }
}