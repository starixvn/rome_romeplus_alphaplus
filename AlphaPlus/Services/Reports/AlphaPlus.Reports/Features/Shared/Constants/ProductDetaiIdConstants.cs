namespace AlphaPlus.Reports.Features.Shared.Constants;

public static class ProductDetaiIdConstants
{
    private const int SoccerPinGoal = 15;
    private const int BasketballPinGoal = 16;
    private const int EsportsPinGoal = 19;
    public const int SabaOtherSports = 24;

    public static readonly HashSet<int> ExcludedProductDetailFor777 =
    [
        SoccerPinGoal,
        BasketballPinGoal,
        EsportsPinGoal
    ];
}