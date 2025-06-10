namespace AlphaPlus.Reports.Features.WinLossDetail.Models;

public class CashFeeItemModel
{
    public string FeeType { get; set; }

    public string Type { get; set; }

    public int Count { get; set; }

    public decimal Fee { get; set; }

    public decimal Agent { get; set; }

    public decimal Company { get; set; }

    public static CashFeeItemModel operator +(CashFeeItemModel model1, CashFeeItemModel model2)
    {
        model1.Count += model2.Count;
        model1.Fee += model2.Fee;
        model1.Agent += model2.Agent;
        model1.Company += model2.Company;

        return model1;
    }
}
