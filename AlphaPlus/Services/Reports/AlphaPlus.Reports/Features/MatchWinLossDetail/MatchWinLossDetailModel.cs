using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class MatchWinLossDetailModel : NetWinLoss
{
    private static readonly int[] ParlayBetGroupIds = [3, 4];

    public int BetGroup { get; set; }

    public bool IsParlay => ParlayBetGroupIds.Contains(BetGroup);

    public int MatchId { get; set; }

    public int HomeId { get; set; }

    public int AwayId { get; set; }

    public int BetType { get; set; }

    public bool LiveIndicator { get; set; }

    public int LeagueId { get; set; }

    public int LeagueGroupId { get; set; }

    public byte LeagueType { get; set; }

    public string Race { get; set; }

    public string MatchCode { get; set; }

    public bool IsNeutral { get; set; }

    public string ShowTime { get; set; }

    public decimal GrossComm { get; set; }

    public static MatchWinLossDetailModel operator +(MatchWinLossDetailModel model1, MatchWinLossDetailModel model2)
    {
        model1.GrossComm += model2.GrossComm;
        model1.Add(model2);

        return model1;
    }
}