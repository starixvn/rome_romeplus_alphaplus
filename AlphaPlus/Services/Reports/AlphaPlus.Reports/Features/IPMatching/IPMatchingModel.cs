namespace AlphaPlus.Reports.Features.IPMatching;

public class IPMatchingModel
{
    public string IP { get; set; }

    public int MemberCount { get; set; }

    public int BetCount { get; set; }

    public decimal Stake { get; set; }

    public decimal MemberWinLoss { get; set; }

    public decimal SuperWinLoss { get; set; }

    public decimal Company { get; set; }

    public static IPMatchingModel operator +(IPMatchingModel model1, IPMatchingModel model2)
    {
        model1.MemberCount += model2.MemberCount;
        model1.BetCount += model2.BetCount;
        model1.Stake += model2.Stake;
        model1.MemberWinLoss += model2.MemberWinLoss;
        model1.SuperWinLoss += model2.SuperWinLoss;
        model1.Company += model2.Company;

        return model1;
    }
}