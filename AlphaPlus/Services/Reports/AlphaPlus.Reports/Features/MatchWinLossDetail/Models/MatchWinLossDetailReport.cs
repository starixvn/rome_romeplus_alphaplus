namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public record MatchWinLossDetailReport(
    bool HasData,
    IEnumerable<MatchWinLossDetailModel> NormalMatchWinLoss,
    BetTypeWinLossModel TotalNormalMatchWinLoss,
    BetTypeWinLossModel MixParlay,
    BetTypeWinLossModel SystemParlay,
    IEnumerable<BetTypeWinLossModel> ParlayMatchWinLoss,
    BetTypeWinLossModel GrandTotal);