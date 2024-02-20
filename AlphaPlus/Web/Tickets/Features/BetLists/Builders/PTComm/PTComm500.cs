using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Builders.PTComm;

public class PTComm500 : PTComm
{
    public PTComm500(ITicket ticket, Core.Models.UserLevel userLevel) : base(ticket, userLevel)
    {
    }

    protected override void InitCommSettingByUserLevel()
    {
        CommSettingByUserLevel = new Dictionary<Core.Models.UserLevel, object>
        {
            [Core.Models.UserLevel.Super] = Ticket.SuperDiscount - Ticket.MasterDiscount,
            [Core.Models.UserLevel.Master] = Ticket.MasterDiscount - Ticket.AgentDiscount,
            [Core.Models.UserLevel.Agent] = -Ticket.AgentDiscount
        };
    }
}