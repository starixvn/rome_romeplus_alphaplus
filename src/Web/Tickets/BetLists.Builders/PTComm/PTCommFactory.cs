using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Builders;

internal static class PTCommFactory
{
    public static PTComm CreateInstance(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
    {
        var ptcomm = BetTypeHelper.IsJackpotContribution(ticket.BetTypeId)
            ? new PTComm2198(ticket, userLevel)
            : ticket.BetTypeId switch
            {
                33 or 1233 => new PTComm33(ticket, ticketData, userLevel),
                500 => new PTComm500(ticket, userLevel),
                501 => new PTComm501(ticket, userLevel),
                71 or 73 => new PTComm71(ticket, userLevel),
                _ => new PTComm(ticket, userLevel)
            };

        return ptcomm.InitPtCommData();
    }
}
