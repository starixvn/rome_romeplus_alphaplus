using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Get;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public class SearchAccountController(IMediator mediator)
        : SiteBaseController(mediator)
    {
        public Task<ActionResult> Index(IndexQuery message) => View(message);

        public Task<ActionResult> GetAccount(GetAccountQuery message) => Json(message);

        public Task<ActionResult> GetDownlineList(DownlineListQuery message) => Json(message);

        public Task<ActionResult> GetLastTxn(GetLastTxnQuery message) => Json(message);

        public Task<ActionResult> GetOnlineList(OnlineListQuery message) => Json(message);

        public Task<ActionResult> GivenCredit(GivenCreditQuery message) => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateGivenCredit([FromBody] GivenCreditCommand command) => Json(command);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateCustomerStatus([FromBody] CustomerNonProductStatusCommand command) => Json(command);
    }
}