using AlphaPlus.Core.Helpers;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.LinkedGames;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class LinkedGamesResultHandler(
    ILinkedGamesResult result,
    IBrowserHelper browserHelper,
    ICacheRefService cacheRefService) : IRequestHandler<Soccer5ResultQuery, ThirdPartyProductViewModel>
{
    public Task<ThirdPartyProductViewModel> Handle(Soccer5ResultQuery message, CancellationToken cancellationToken)
    {
        var url = result.GetSoccer5Result(message.MatchId, message.Balls, IsDesktop(), message.IsDarkMode, message.Language);
        var viewModel = new ThirdPartyProductViewModel(
            $"{cacheRefService.GetSportName(message.SportTypeId)} - {cacheRefService.GetBetTypeName(message.BetTypeId)}",
            url);

        return Task.FromResult(viewModel);
    }

    private bool IsDesktop() => !browserHelper.IsMobile();
}