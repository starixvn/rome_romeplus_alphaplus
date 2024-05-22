using AlphaPlus.Core.Helpers;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class KenoResultHandler(IKenoResult kenoResult, IBrowserHelper browserHelper)
    : IRequestHandler<KenoResultQuery, KenoResultViewModel>
{
    public Task<KenoResultViewModel> Handle(KenoResultQuery message, CancellationToken cancellationToken)
    {
        var result = kenoResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new KenoResultViewModel(result);

        return Task.FromResult(viewModel);
    }
}