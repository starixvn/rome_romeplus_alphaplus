using AlphaPlus.BetLists.BetBuilder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetBuilder;

public class BetBuilderDetailViewModel(IEnumerable<BetBuilderDetailModel> details, string status)
{
    private readonly IEnumerable<string> SettledStatus =
    [
        BetStatus.Won,
        BetStatus.Lose,
        BetStatus.Refund,
        BetStatus.Draw
    ];

    public IEnumerable<BetBuilderDetailModel> Details { get; } = details;

    public bool IsSettled => SettledStatus.Any(i => i.Equals(Status, StringComparison.OrdinalIgnoreCase));

    private string Status { get; } = status;
}
