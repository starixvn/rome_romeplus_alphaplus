namespace AlphaPlus.Reports.Features.WhiteLabel;

public class DailyWinLossModel
{
    public DateTime WinLostDate { get; init; }

    public string WhiteLabelName { get; init; }

    public string Agent { get; init; }

    public string Master { get; init; }

    public decimal MemberWinLoss { get; set; }

    public decimal Promotion { get; set; }

    public static DailyWinLossModel operator +(DailyWinLossModel model1, DailyWinLossModel model2)
    {
        model1.MemberWinLoss += model2.MemberWinLoss;
        model1.Promotion += model2.Promotion;

        return model1;
    }
}