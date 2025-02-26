namespace AlphaPlus.Reports.Features.WinLossByProduct;

public class WinLossByBetTypeModel
{
    public int BetType { get; init; }

    public string BetTypeName { get; set; }

    public int ProductId { get; init; }

    public int SportType { get; init; }

    public string SportTypeName { get; set; }

    public bool LiveIndicator { get; init; }

    public bool IsMMR { get; init; }

    public int NumberOfTickets { get; set; }

    public decimal Turnover { get; set; }

    public decimal GrossComm { get; set; }

    public decimal MemberWinLoss { get; set; }

    public decimal MemberComm { get; set; }

    public decimal MemberTotal
    {
        get => MemberWinLoss + MemberComm;
        private set => _ = value;
    }

    public decimal DownlineTotal { get; set; }

    public decimal WinLoss { get; set; }

    public decimal Comm { get; set; }

    public decimal Total
    {
        get => WinLoss + Comm;
        private set => _ = value;
    }

    public decimal Company
    {
        get => -(MemberTotal + DownlineTotal + Total);
        private set => _ = value;
    }

    public static WinLossByBetTypeModel operator +(WinLossByBetTypeModel model1, WinLossByBetTypeModel model2)
    {
        model1.NumberOfTickets += model2.NumberOfTickets;
        model1.Turnover += model2.Turnover;
        model1.GrossComm += model2.GrossComm;
        model1.MemberWinLoss += model2.MemberWinLoss;
        model1.MemberComm += model2.MemberComm;
        model1.MemberTotal += model2.MemberTotal;
        model1.DownlineTotal += model2.DownlineTotal;
        model1.WinLoss += model2.WinLoss;
        model1.Comm += model2.Comm;
        model1.Total += model2.Total;
        model1.Company += model2.Company;

        return model1;
    }
}