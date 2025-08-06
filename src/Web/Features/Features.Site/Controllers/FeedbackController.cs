using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Site.Messages.Requests.Feedback;
using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Features.Site.Requests.Feedback;

namespace AlphaPlus.Features.Site.Messages.Controllers;

[AllowProtectedMode]
public class FeedbackController : SiteBaseController
{
    public FeedbackController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public ViewResult Index() => View();

    [HttpGet]
    public Task<ActionResult> CheckExist(CheckExistsFeedbackQuery query)
        => JsonAsync<CheckExistsFeedbackViewModel>(query);

    [HttpGet]
    public async Task<ActionResult> Function(GetFunctionFeedbackQuery query)
    {
        var viewModel = await mediator.Send(query);

        if (viewModel.FunctionId == 0)
        {
            return new EmptyResult();
        }

        return View(viewModel);
    }

    public Task<ActionResult> Add(AddFeedbackQuery query) => JsonAsync(query);

    public Task<ActionResult> AddSurvey(AddFeedbackQuery query) => JsonAsync(query);

    [HttpGet]
    public ViewResult WinningThresholdSurvey(FeedbackSurveyQuery query)
    {
        var viewModel = new FeedbackSurveyViewModel(query);
        return View(viewModel);
    }

    public ViewResult Dislike(DislikeQuery message) => View(message);
}
