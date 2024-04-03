using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.Shared.Resources;

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

        Name = $"{GetBetTypePrefix()}{betTypeName ?? string.Empty}{GetBetTypeSuffix()}";
    }

    public int Id { get; }

    public string Name { get; } = string.Empty;

    public decimal GrossComm { get; }

    public decimal CompanyComm { get; }

    public bool IsLive { get; }

    public int Race { get; }

    private string GetBetTypePrefix()
        => Id switch
        {
            BetTypeConstants.N_Over_Under => $"{SharedResources.Next} ",
            BetTypeConstants.N_Odd_Even => IsLive ? $"{SharedResources.Next} " : $"{SharedResources.FT} ",
            _ => string.Empty
        };

    private string GetBetTypeSuffix()
        => Id == BetTypeConstants.MixParlayBetRadarVirtualSports || Id == BetTypeConstants.MixParlayEdgeGamingVirtualSports
        ? $" ({SharedResources.VirtualSports})"
        : string.Empty;
}