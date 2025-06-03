namespace AlphaPlus.Reports.Features.WinLossDetail.Models;

public class CashFeeModel
{
    public List<CashFeeGroupModel> FeeGroups { get; set; } = [];

    public CashFeeItemModel GrandTotal { get; set; }
}
