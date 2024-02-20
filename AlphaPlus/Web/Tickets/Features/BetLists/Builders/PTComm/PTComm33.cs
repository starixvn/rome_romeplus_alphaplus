using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Builders.PTComm;

public class PTComm33 : PTComm
{
    public PTComm33(ITicket ticket, List<ITicketData> ticketData, Core.Models.UserLevel userLevel)
        : base(ticket, userLevel)
    {
        TicketData = ticketData;
        PositionTakingWin = GetPositionTakingByUserLevel(userLevel, isWin: true);
        PositionTakingPlace = GetPositionTakingByUserLevel(userLevel, isWin: false);
    }

    protected List<ITicketData> TicketData { get; }

    public string PositionTakingWin { get; }

    public string PositionTakingPlace { get; }

    public int WinBetId => !IsVirtualRacing ? 31 : 1231;

    public int PlaceBetId => !IsVirtualRacing ? 32 : 1232;

    private bool IsVirtualRacing => Ticket.BetTypeId == 1233;

    private string GetPositionTakingByUserLevel(Core.Models.UserLevel userLevel, bool isWin)
    {
        var refTicketData = IsVirtualRacing
            ? TicketData.FindAll(item => item.RefNo.Equals(Ticket.TransId.ToString()))
            : TicketData.FindAll(item => item.RefNo.Equals(Ticket.RefNo));
        var refTicketDataItem = refTicketData.LastOrDefault(item => item.BetTypeId == (isWin ? WinBetId : PlaceBetId));

        var positionTaking = 0M;

        if (refTicketDataItem != null)
        {
            if (userLevel.IsSuperLevel())
            {
                positionTaking = refTicketDataItem.SuperPT;
            }
            else if (userLevel.IsMasterLevel())
            {
                positionTaking = refTicketDataItem.MasterPT;
            }
            else
            {
                positionTaking = refTicketDataItem.AgentPT;
            }
        }

        return Formatter.FormatNumber2(positionTaking, null) + "%";
    }
}
