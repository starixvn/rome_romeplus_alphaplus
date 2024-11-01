using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;

public class ComboParlayViewModel : BetListBaseViewModel
{
    public ComboParlayViewModel(
        ComboParlayQuery message,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper) : base(message, betListData, builder, ticketHelper, supportRiskAmount: true)
    {
        TicketHelper.IsShowOddsWithChoice = true;
        HideOfferNameUnitStakes = string.IsNullOrEmpty(message.BetCheck);
        ComboParlayOfferNames = string.IsNullOrEmpty(message.BetCheck)
            ? []
            : TicketHelper.GetCombinationMPOfferNameList(message.BetTeam, message.BetCheck, message.Handicap1, message.Handicap2);
        ComboParlaySingleTickets = BetListData.Tickets
            .Where(t => t.Stake > 0)
            .OrderBy(t => t.MatchId)
            .ToList();
        UnitStakeByOfferNames = GetUnitStakeByOfferName(message);
    }

    public IDictionary<string, string> UnitStakeByOfferNames { get; }

    private IList<string> ComboParlayOfferNames { get; }

    private List<TicketDto> ComboParlaySingleTickets { get; }

    private bool HideOfferNameUnitStakes { get; }

    public bool HasValidData
        => ComboParlaySingleTickets.Any()
        && (HideOfferNameUnitStakes || ComboParlayOfferNames.Count <= ComboParlaySingleTickets.Count);

    private IDictionary<string, string> GetUnitStakeByOfferName(ComboParlayQuery message)
    {
        var offerNameUnitStakes = new Dictionary<string, string>();
        if (HideOfferNameUnitStakes)
        {
            return offerNameUnitStakes;
        }

        var isSystemParlay = message.Handicap1 == 0;

        if (HasValidData)
        {
            var offerNamesAmount = ComboParlayOfferNames.Count;
            for (var i = 0; i < offerNamesAmount; i++)
            {
                var comboTicket = isSystemParlay ? ComboParlaySingleTickets[0] : ComboParlaySingleTickets[i];

                offerNameUnitStakes.Add(ComboParlayOfferNames[i], comboTicket.Currency + " " + comboTicket.Stake.ToString("N0"));
            }
        }

        return offerNameUnitStakes;
    }
}
