using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using AlphaPlus.Settings.Site.Features.CopyAccount.Request;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace AlphaPlus.Settings.Site.Features.CopyAccount
{
    public class CopyAccountController(IMediator mediator) : SiteBaseController(mediator)
    {
        public async Task<ActionResult> Index(CopyAccountQuery query)
            => await ViewAsync(query);

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
                return PartialView("_CopyResult", result);
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

        public async Task<ActionResult> CheckExistedCopyCustomer(CheckExistedCustomerQuery query)
            => await JsonAsync(query);

        public async Task<ActionResult> CheckExistedCustomer([FromBody] CheckExistedCustomerQuery query)
            => await JsonAsync(query);
    }
}