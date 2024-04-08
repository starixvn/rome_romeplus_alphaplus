namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Features.Site.Messages.Requests.Feedback;
    using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
    using MediatR;
    using AlphaPlus.Core.Filters;

    public class FeedbackController : SiteBaseController
    {
        public FeedbackController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet, AllowProtectedMode]
        public ViewResult Index() => View();

        [HttpGet, AllowProtectedMode]
        public Task<ActionResult> CheckExist(CheckExistsFeedbackQuery query)
            => JsonAsync<CheckExistsFeedbackViewModel>(query);

        [HttpGet, AllowProtectedMode]
        public async Task<ActionResult> Function(GetFunctionFeedbackQuery query)
        {
            var viewModel = await mediator.Send(query);

            if (viewModel.FunctionId == 0)
            {
                return new EmptyResult();
            }

            return View(viewModel);
        }

        [AllowProtectedMode]
        public Task<ActionResult> Add(AddFeedbackQuery query) => JsonAsync(query);
    }
}