using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTEntity : NetWinLoss
{
    public decimal SuperPT { get; set; }

    public int ProductId { get; init; }

    public string ProductName { get; set; }

    public decimal NetTurnover { get; set; }

    public decimal GrossComm { get; set; }

    public static SuperWinLossDetailByPTEntity operator +(SuperWinLossDetailByPTEntity model1, SuperWinLossDetailByPTEntity model2)
    {
        model1.NetTurnover += model2.NetTurnover;
        model1.GrossComm += model2.GrossComm;
        model1.Add(model2);

        return model1;
    }
}