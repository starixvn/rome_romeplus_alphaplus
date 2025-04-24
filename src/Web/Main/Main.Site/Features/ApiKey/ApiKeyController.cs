using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Main.Site.Features.ApiKey
{
    public class ApiKeyController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        public IActionResult CreateApiKey()
        {
            var viewModel = new CreateApiKeyViewModel(Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName, userSession.Language);
            return View(viewModel);
        }

        public async Task<IActionResult> ApiKeyManagement(GetCustomerApiKeyQuery query)
        {
            var viewModel = await mediator.Send(query);
            return View(viewModel);
        }

        public async Task<IActionResult> EditApiKey(GetApiKeyQuery query)
        {
            var viewModel = await mediator.Send(query);
            return View(viewModel);
        }

        public IActionResult CreateApiKeyResult(string apiName, string key, string secret)
        {
            var viewModel = new CreateApiKeyResultViewModel(apiName, key, secret);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApiKey(CreateApiKeyCommand query)
        {
            return Json(await mediator.Send(query));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeApiKey(RevokeApiKeyCommand query)
        {
            return Json(await mediator.Send(query));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApiKey(UpdateApiKeyCommand query)
        {
            return Json(await mediator.Send(query));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (userSession.IsSubAcc
                || userSession.IsProtectedMode
                || (!AppSettings.APISuperIds.Any(s => string.Equals(s, "*")) &&
                    !AppSettings.APISuperIds.Any(s => string.Equals(s, userSession.SRecommendId.ToString()))))
            {
                context.Result = new UnauthorizedObjectResult(new { message = CommonResx.NotAuthorized });
            }
        }
    }
}