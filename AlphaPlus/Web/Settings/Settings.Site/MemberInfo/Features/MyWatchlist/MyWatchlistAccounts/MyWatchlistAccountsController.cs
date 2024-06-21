namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts
{
    using AlphaPlus.Settings.Site.Controllers;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class MyWatchlistAccountsController : SiteBaseController
    {
        private const string IndexPath = "~/MemberInfo/Features/MyWatchlist/MyWatchlistAccounts/Views/Index.cshtml";
        private const string ManageWatchlistAccountsPath = "~/MemberInfo/Features/MyWatchlist/MyWatchlistAccounts/Views/ManageWatchlistAccounts.cshtml";
        private const string ManageGlobalThresholdPath = "~/MemberInfo/Features/MyWatchlist/MyWatchlistAccounts/Views/ManageGlobalThreshold.cshtml";
        private const string ManagePersonalThresholdPresetPath = "~/MemberInfo/Features/MyWatchlist/MyWatchlistAccounts/Views/ManagePersonalThresholdPreset.cshtml";

        public MyWatchlistAccountsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MyWatchlistAccountsQuery message)
            => await ViewAsync(message, IndexPath);

        public async Task<ActionResult> ManageWatchlistAccounts(ManageWatchlistAccountsQuery message)
            => await ViewAsync(message, ManageWatchlistAccountsPath);

        public async Task<ActionResult> ManageGlobalThreshold(ManageGlobalThresholdQuery message)
            => await ViewAsync(message, ManageGlobalThresholdPath);

        public async Task<ActionResult> ManagePersonalThresholdPreset(ManagePersonalThresholdPresetQuery message)
            => await ViewAsync(message, ManagePersonalThresholdPresetPath);

        [HttpPost]
        public async Task<ActionResult> GetWatchlistAccounts(WatchlistAccountsQuery message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPersonalThreshold(AddPersonalThresholdCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMultiplePersonalThreshold(UpdateMultiplePersonalThresholdCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteMultiplePersonalThreshold(DeleteMultiplePersonalThresholdCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateGlobalThreshold(UpdateGlobalThresholdCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMultiplePersonalPreset(AddMultiplePersonalPresetCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePersonalPreset(UpdatePersonalPresetCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePersonalPreset(DeletePersonalPresetCommand message)
        {
            if (!message.IsValid())
            {
                return Json(Result.CreateErrorResult(Lang.NotAuthorized));
            }

            return Json(await mediator.Send(message));
        }
    }
}