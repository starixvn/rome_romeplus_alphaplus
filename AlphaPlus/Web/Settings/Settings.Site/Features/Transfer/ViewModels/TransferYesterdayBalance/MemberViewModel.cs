using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.Transfer.API.Models.TransferYesterdayBalance;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;

public class MemberViewModel(string actorName, MemberTransfer member)
{
    public string ActorName { get; } = actorName;

    public string Amount { get; } = (-member.Balance).FormatNumber4();

    public MemberTransfer Member { get; } = member;
}