namespace AlphaPlus.Reports.Features.WinLossByProduct;

public class WinLossByProductModel(
    int productId,
    string productName,
    IEnumerable<WinLossBySportModel> winLossBySports,
    WinLossByBetTypeModel total)
{
    public int ProductId { get; } = productId;

    public string ProductName { get; } = productName;

    public IEnumerable<WinLossBySportModel> WinLossBySports { get; } = winLossBySports;

    public WinLossByBetTypeModel Total { get; } = total;
}