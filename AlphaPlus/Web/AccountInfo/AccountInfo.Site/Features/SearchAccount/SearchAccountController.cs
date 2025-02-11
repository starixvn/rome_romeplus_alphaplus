using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Get;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Shared.AppResources.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public class SearchAccountController(IMediator mediator)
        : SiteBaseController(mediator)
    {
        public Task<ActionResult> Index(IndexQuery message) => View(message);

        public Task<ActionResult> GetCustomerInfo(CustomerInfoQuery message) => Json(message);

        public Task<ActionResult> GetTotalDownlines(TotalDownlineQuery message)
            => Json(message);

        public Task<ActionResult> GetDownlineList(DownlineListQuery message) => Json(message);

        public Task<ActionResult> GetDirectMembers(DirectMemberListQuery message)
            => Json(message);

        public Task<ActionResult> GetLastTxn(GetLastTxnQuery message) => Json(message);

        public Task<ActionResult> GetOnlineList(OnlineListQuery message) => Json(message);

        public Task<ActionResult> GivenCredit(GivenCreditQuery message)
        {
            ViewBag.Title = $"{CommonResx.GivenCreditPopupTitle} - {message.SearchAccountName}";

            return View(message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateGivenCredit([FromBody] GivenCreditCommand command) => Json(command);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateCustomerStatus([FromBody] CustomerNonProductStatusCommand command) => Json(command);

        public async Task<ActionResult> GetMemberBalanceInfo(MemberBalanceInfoQuery message)
        {
            ViewBag.Title = $"{CommonResx.BalanceInfo} - {message.CustName}";
            return await View(message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateAllDayBalanceTransfer([FromBody] UpdateAllDayBalanceTransferRequest command)
           => Json(command);
    }
}