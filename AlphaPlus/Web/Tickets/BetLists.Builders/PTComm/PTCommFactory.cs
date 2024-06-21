using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.BetLists.Builders;

public static class PTCommFactory
{
    private const int JackpotContribution = 2198;
    private const int JackpotContribution_Mobile = 2398;
    private const int JackpotContribution_Tablet = 2998;
    private const int PPJackpotContribution = 4297;
    private const int SGJackpotContribution = 4599;

    public static PTComm CreateInstance(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
    {
        if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
        {
            return new PTComm33(ticket, ticketData, userLevel).InitPtCommData();
        }

        if (ticket.BetTypeId == 500)
        {
            return new PTComm500(ticket, userLevel).InitPtCommData();
        }

        if (ticket.BetTypeId == 501)
        {
            return new PTComm501(ticket, userLevel).InitPtCommData();
        }

        if (ticket.BetTypeId == 71 || ticket.BetTypeId == 73)
        {
            return new PTComm71(ticket, userLevel).InitPtCommData();
        }

        if (IsJackpotContribution(ticket.BetTypeId))
        {
            return new PTComm2198(ticket, userLevel).InitPtCommData();
        }

        return new PTComm(ticket, userLevel).InitPtCommData();
    }

    private static bool IsJackpotContribution(int betType)
        => JackpotContribution == betType
        || JackpotContribution_Mobile == betType
        || JackpotContribution_Tablet == betType
        || PPJackpotContribution == betType
        || SGJackpotContribution == betType;
}
