namespace AlphaPlus.Reports.Features.CashBillingService.Models;

public class CashFeeModel
{
    public List<CashFeeGroupModel> FeeGroups { get; set; } = [];

    public CashFeeItemModel GrandTotal { get; set; }

    public bool IsNegative(decimal? cellValue) => cellValue.HasValue && cellValue.Value < 0;
}
