using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class BetTypeWinLossModel : NetWinLoss
{
    public BetTypeWinLossModel(MatchWinLossDetailEntity winloss) : this(winloss, null)
    {
    }

    public BetTypeWinLossModel(MatchWinLossDetailEntity winloss, string betTypeName)
    {
        Id = winloss.BetType;
        Name = betTypeName;
        Turnover = winloss.Turnover;
        GrossComm = winloss.GrossComm;
        IsLive = winloss.LiveIndicator;
        Race = winloss.Race;
        MemberWinLoss = winloss.MemberWinLoss;
        MemberComm = winloss.MemberComm;
        AgWinLoss = winloss.AgWinLoss;
        AgComm = winloss.AgComm;
        MAgWinLoss = winloss.MAgWinLoss;
        MAgComm = winloss.MAgComm;
        SAgWinLoss = winloss.SAgWinLoss;
        SAgComm = winloss.SAgComm;
        CompanyComm = winloss.CompanyComm;
    }

    public int Id { get; }

    public string Name { get; }

    public decimal GrossComm { get; }

    public decimal CompanyComm { get; }

    public bool IsLive { get; }

    public int Race { get; }
}