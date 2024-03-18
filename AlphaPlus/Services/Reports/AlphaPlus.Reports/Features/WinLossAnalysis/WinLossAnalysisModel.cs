using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.WinLossAnalysis;

public class WinLossAnalysisModel : NetWinLoss
{
    public int? RollupKeyId { get; set; }

    public string RollupKey { get; set; }

    public int BetCount { get; set; }

    public static WinLossAnalysisModel operator +(WinLossAnalysisModel model1, WinLossAnalysisModel model2)
    {
        model1.BetCount += model2.BetCount;
        model1.Add(model2);

        return model1;
    }
}