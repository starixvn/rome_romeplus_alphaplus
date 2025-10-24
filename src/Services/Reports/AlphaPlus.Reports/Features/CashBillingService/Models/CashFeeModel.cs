namespace AlphaPlus.Reports.Features.CashBillingService.Models;

public class CashFeeModel
{
    public List<CashFeeGroupModel> FeeGroups { get; set; } = [];

    public CashFeeItemModel GrandTotal { get; set; }

    public bool IsNegative(decimal? cellValue) 
    {
        if (!cellValue.HasValue) 
        {
            return false;
        }
        var rounded = Math.Round(cellValue.Value, 2, MidpointRounding.AwayFromZero);
        return rounded < 0;
    }
}
