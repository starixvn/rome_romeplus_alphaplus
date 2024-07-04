namespace AlphaPlus.BetLists.Shared.Constants;

internal static class ProductIdConstants
{
    private const int LiveCasino = 41;
    private const int RNGCasino = 53;

    public static readonly HashSet<int> IgnoreCheckProductStatusTypes = [LiveCasino, RNGCasino];
}