using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;

namespace AlphaPlus.BetLists.Builders;

public class WinLossBuilder
{
    public WinLossBuilder(ITicket ticket)
    {
        SetData(ticket);
        CommVisibility = DisplayComm(ticket);
    }

    public string WinLoss { get; private set; }

    public string Comm { get; private set; }

    public bool CommVisibility { get; }

    private static bool DisplayComm(ITicket ticket)
        => !BetTypeHelper.IsJackpotContribution(ticket.BetTypeId);

    private void SetData(ITicket ticket)
    {
        if (BetTypeHelper.IsJackpotContribution(ticket.BetTypeId))
        {
            WinLoss = "-";
            Comm = string.Empty;

            return;
        }

        var status = ticket.Status.ToLowerInvariant();

        switch (status)
        {
            case BetStatus.Running:
            case BetStatus.Waiting:
            case BetStatus.Pending:
                WinLoss = "-";
                Comm = string.Empty;
                break;

            case BetStatus.Void:
                WinLoss = ticket.BetTypeId == 500 ? "(NW) 0.00" : string.Empty;
                Comm = "0.00";
                break;

            case BetStatus.Reject:
                WinLoss = "0.00";
                Comm = string.Empty;
                break;

            default:
                WinLoss = ticket.BetTypeId == 500 ? "(NW) " + ConvertByBetType.Winloss(ticket.ActualStake) : ConvertByBetType.Winloss(ticket.Winlost);
                Comm = ticket.BetTypeId == 500 ? ConvertByBetType.Winloss(ticket.PlayerComm) : ConvertByBetType.Comm(ticket.PlayerComm);
                break;
        }
    }
}