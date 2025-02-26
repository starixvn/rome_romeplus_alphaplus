namespace AlphaPlus.Reports.Features.CommissionByBetType;

public class CommissionByBetTypeModel
{
    public int CustId { get; init; }

    public string CustName { get; init; }

    public decimal Comm1 { get; set; }

    public decimal Comm2 { get; set; }

    public decimal Comm3 { get; set; }

    public decimal CasinoComm { get; set; }

    public static CommissionByBetTypeModel operator +(CommissionByBetTypeModel model1, CommissionByBetTypeModel model2)
    {
        model1.Comm1 += model2.Comm1;
        model1.Comm2 += model2.Comm2;
        model1.Comm3 += model2.Comm3;
        model1.CasinoComm += model2.CasinoComm;

        return model1;
    }
}