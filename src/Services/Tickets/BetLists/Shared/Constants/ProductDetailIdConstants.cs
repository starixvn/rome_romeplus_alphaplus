namespace AlphaPlus.BetLists.Shared.Constants;

internal static class ProductDetailIdConstants
{
    private const int SoccerPinGoal = 15;
    private const int BasketballPinGoal = 16;
    private const int EsportsPinGoal = 19;

    public static readonly HashSet<int> ExcludedProductDetailFor777 =
    [
        SoccerPinGoal,
        BasketballPinGoal,
        EsportsPinGoal
    ];
}