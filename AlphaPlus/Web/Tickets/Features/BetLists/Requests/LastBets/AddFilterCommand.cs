using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.Shared.Constants;
using AlphaPlus.BetLists.Shared.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class AddFilterCommand : AddUnsavedFilterCommand, IRequest<Result>
{
    public string BetType { get; set; }

    public int Stake { get; set; }

    public SportXml[] Sports { get; set; }

    public byte LiveStatus { get; set; }

    public byte FilterByMembers { get; set; }

    public string InputList { get; set; }

    public string InputListMembers { get; set; }

    public string MyWatchlist { get; set; }

    public string MyWatchlistMembers { get; set; }

    public override bool IsValid()
        => base.IsValid()
        && !string.IsNullOrWhiteSpace(BetType)
        && Stake >= 0
        && IsValidLiveStatus()
        && Sports?.Length > 0
        && IsValidFilterByMembers();

    private bool IsValidLiveStatus()
        => LiveStatus == LiveStatusConstants.All
        || LiveStatus == LiveStatusConstants.Live
        || LiveStatus == LiveStatusConstants.NonLive;

    private bool IsValidFilterByMembers()
    {
        if (FilterByMembers == FilterByMembersConstants.AllMembers)
        {
            return InputList == "0" && MyWatchlist == "0";
        }

        if (FilterByMembers == FilterByMembersConstants.AllWatchlistMembers)
        {
            return MyWatchlist == "0";
        }

        return !string.IsNullOrWhiteSpace(InputList) || !string.IsNullOrWhiteSpace(MyWatchlist);
    }
}
