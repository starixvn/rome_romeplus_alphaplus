using Alpha.MemberInfo.Site.Controllers;
using Alpha.MemberInfo.Site.Features.CopyAccount.Request;
using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Alpha.MemberInfo.Site.Features.CopyAccount
{
    public class CopyAccountController : SiteBaseController
    {
        private const string IndexPath = "~/Features/CopyAccount/Views/Index.cshtml";
        private const string ResultPath = "~/Features/CopyAccount/Views/_CopyResult.cshtml";

        public CopyAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CopyAccountQuery query)
            => await ViewAsync(query, IndexPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CopyAccountCommand command)
        {
            if (!command.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized, BadRequestErrorCode));
            }

            var result = await mediator.Send(command);
            if (result.Usernames.Any())
            {
                result.Credit = command.Credit;
                result.NoOfAccount = command.NumberOfAccounts;
                return PartialView(ResultPath, result);
            }
            else
            {
                return Json(Result.CreateErrorResult(result.Message, result.Code));
            }
        }

        public async Task<ActionResult> GetCredit(CreditQuery query)
            => await JsonAsync(query);

        public async Task<ActionResult> CheckUsername(CheckUsernameQuery query)
            => await JsonAsync(query);

        public async Task<ActionResult> CheckExistedCustomer(CheckExistedCustomerQuery query)
            => await JsonAsync(query);
    }
}