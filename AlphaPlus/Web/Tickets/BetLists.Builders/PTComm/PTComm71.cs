using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Builders;

internal class PTComm71(ITicket ticket, UserLevel userLevel) : PTComm(ticket, userLevel)
{
    public override bool VisibilePositionTakingPercent => false;

    public override bool VisibileCommisionPercent => false;
}
