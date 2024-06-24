namespace AlphaPlus.BetLists.Builders;

internal static class BetTypeHelper
{
    private const int JackpotContribution = 2198;
    private const int JackpotContribution_Mobile = 2398;
    private const int JackpotContribution_Tablet = 2998;
    private const int PPJackpotContribution = 4297;
    private const int SGJackpotContribution = 4599;

    private static readonly HashSet<int> JackpotContributionBetTypes =
    [
        JackpotContribution,
        JackpotContribution_Mobile,
        JackpotContribution_Tablet,
        PPJackpotContribution,
        SGJackpotContribution
    ];

    public static bool IsJackpotContribution(int betType) => JackpotContributionBetTypes.Contains(betType);
}