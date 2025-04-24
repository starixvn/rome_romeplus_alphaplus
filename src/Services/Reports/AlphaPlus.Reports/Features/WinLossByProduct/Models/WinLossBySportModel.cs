using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.Features.WinLossByProduct;

public class WinLossBySportModel(
    int sportType,
    string sportTypeName,
    IEnumerable<WinLossByBetTypeModel> winLossByBetTypes,
    WinLossByBetTypeModel total)
{
    public int SportType { get; } = sportType;

    public string SportTypeName { get; } = sportTypeName;

    public IEnumerable<WinLossByBetTypeModel> WinLossByBetTypes { get; } = winLossByBetTypes;

    public WinLossByBetTypeModel Total { get; } = total;

    public static bool IsShowBetTypeDirectly(int productId)
        => ProductIdConstants.VirtualSports != productId;
}