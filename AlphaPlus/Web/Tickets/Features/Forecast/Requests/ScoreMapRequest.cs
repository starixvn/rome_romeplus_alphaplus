using AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class ScoreMapRequest : IRequest<ScoreMapViewModel>
{
    public int ViewerId { get; set; } = 23003140;

    public string Language { get; set; } = "en-US";
}

public class ScoreMapRequestHandler : IRequestHandler<ScoreMapRequest, ScoreMapViewModel>
{
    private readonly IScoreMapService scoreMapService;

    public ScoreMapRequestHandler(IScoreMapService scoreMapService)
        => this.scoreMapService = scoreMapService;

    public async Task<ScoreMapViewModel> Handle(ScoreMapRequest message, CancellationToken cancellationToken)
    {
        var betTypesTask = scoreMapService.GetBetTypes(message.Language);
        var leaguesTask = scoreMapService.GetLeagues(message.ViewerId, message.Language);
        await Task.WhenAll(betTypesTask, leaguesTask);

        return new ScoreMapViewModel(await betTypesTask, await leaguesTask);
    }
}