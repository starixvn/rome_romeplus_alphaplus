using AlphaPlus.Core.Helpers;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class VirtualGamesResultHandler(IVirtualGamesResult virtualGamesResult, IBrowserHelper browserHelper)
    : IRequestHandler<VirtualGamesResultQuery, VirtualGamesResultViewModel>
{
    public Task<VirtualGamesResultViewModel> Handle(VirtualGamesResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = virtualGamesResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new VirtualGamesResultViewModel(resultPath);

        return Task.FromResult(viewModel);
    }
}