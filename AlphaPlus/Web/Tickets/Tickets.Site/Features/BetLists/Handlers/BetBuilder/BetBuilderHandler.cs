using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.BetBuilder;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilder;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetBuilder;
using AlphaPlus.Tickets.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetBuilder;

public class BetBuilderHandler(
    IBetBuilderService betBuilderService,
    ICacheRefService cacheRefService)
    : IRequestHandler<BetBuilderDetailQuery, BetBuilderDetailViewModel>
{
    private readonly IBetBuilderService betBuilderService = betBuilderService;

    public async Task<BetBuilderDetailViewModel> Handle(BetBuilderDetailQuery message, CancellationToken cancellationToken)
    {
        var details = await betBuilderService.GetBetList(message.CustomerId, message.TransactionId);

        return new BetBuilderDetailViewModel(BuildBetBuilderDetailModel(details), message.Status);
    }

    private IEnumerable<BetBuilderDetailModel> BuildBetBuilderDetailModel(IEnumerable<BetBuilderDetailModel> details)
    {
        foreach (var item in details)
        {
            var names = cacheRefService.GetBetBuilderSelectionNames([item.SelectionType]);
            item.SelectionName = names != null && names.Length == 1 ? names[0] : item.SelectionType.ToString();
            item.Result = string.IsNullOrWhiteSpace(item.Result)
                ? item.Result
                : Resources.ResourceManager.GetString(item.Result.ToLowerInvariant());

            yield return item;
        }
    }
}
