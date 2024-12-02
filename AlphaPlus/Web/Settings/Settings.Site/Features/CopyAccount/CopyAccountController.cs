using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.CopyAccount.Request;
using AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.CopyAccount
{
    public class CopyAccountController(IMediator mediator) : SiteBaseController(mediator)
    {
        public Task<ActionResult> Index(CopyAccountQuery query)
            => View(query);

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
                return PartialView("_CopyResult", new CopyAccountResultViewModel
                {
                    Result = result,
                    Credit = command.Credit,
                    NumberOfAccounts = command.NumberOfAccounts,
                    NoOfFailedAccount = command.NumberOfAccounts - result.Usernames.Count()
                });
            }
            else
            {
                return Json(Result.CreateErrorResult(result.Message, result.Code));
            }
        }

        public Task<ActionResult> GetCredit(CreditQuery query)
            => Json(query);

        public Task<ActionResult> CheckUsername(CheckUsernameQuery query)
            => Json(query);
    }
}