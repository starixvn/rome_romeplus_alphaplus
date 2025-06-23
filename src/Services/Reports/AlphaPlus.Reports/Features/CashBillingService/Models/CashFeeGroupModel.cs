namespace AlphaPlus.Reports.Features.CashBillingService.Models;

public class CashFeeGroupModel
{
    public string GroupName { get; set; }

    public List<CashFeeItemModel> FeeItems { get; set; } = [];

    public CashFeeItemModel Total { get; set; }
}
