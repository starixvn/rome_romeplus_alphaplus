using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Builders;

public class PTComm500(ITicket ticket, UserLevel userLevel) : PTComm(ticket, userLevel)
{
    protected override void InitCommSettingByUserLevel()
    {
        CommSettingByUserLevel = new Dictionary<UserLevel, object>
        {
            [UserLevel.Super] = Ticket.SuperDiscount - Ticket.MasterDiscount,
            [UserLevel.Master] = Ticket.MasterDiscount - Ticket.AgentDiscount,
            [UserLevel.Agent] = -Ticket.AgentDiscount
        };
    }
}