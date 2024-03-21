using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class BetTypeWinLossModel : NetWinLoss
{
    public BetTypeWinLossModel(MatchWinLossDetailModel winloss, string betTypeName)
    {
        Id = winloss.BetType;
        Name = betTypeName;
        Turnover = winloss.Turnover;
        GrossComm = winloss.GrossComm;
        IsLive = winloss.LiveIndicator;
        Race = winloss.Race;
        MemberWinLoss = winloss.MemberWinLoss;
        MemberComm = winloss.MemberComm;
    }

    public int Id { get; }

    public string Name { get; }

    public decimal GrossComm { get; }

    public bool IsLive { get; }

    public int Race { get; }
}