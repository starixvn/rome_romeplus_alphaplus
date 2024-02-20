using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Builders.PTComm;

public class PTComm71 : PTComm
{
    public PTComm71(ITicket ticket, Core.Models.UserLevel userLevel) : base(ticket, userLevel)
    {
    }

    public override bool VisibilePositionTakingPercent => false;

    public override bool VisibileCommisionPercent => false;
}
