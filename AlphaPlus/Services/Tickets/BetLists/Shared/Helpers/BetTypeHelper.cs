using AlphaPlus.BetLists.AppResources;

namespace AlphaPlus.BetLists.Shared.Helpers;

public static class BetTypeHelper
{
    public const int MonitoringBetsOthers = -1;

    private static readonly int[] JackpotContributionBetType =
    {
        BetTypeConstants.JackpotContribution,
        BetTypeConstants.JackpotContribution_Mobile,
        BetTypeConstants.JackpotContribution_Tablet,
        BetTypeConstants.PPJackpotContribution,
        BetTypeConstants.SGJackpotContribution
    };

    private static readonly int[] HorseRacingBetType =
    {
        BetTypeConstants.VirtualHorseRacing_Win,
        BetTypeConstants.VirtualHorseRacing_Place,
        BetTypeConstants.VirtualHorseRacing_WinPlace
    };

    public static bool IsOutRight(int betType) => betType == BetTypeConstants.Outright;

    public static bool IsLeapGaming(int betType)
        => BetTypeConstants.LeapGamingMin == betType;

    public static bool IsHappy5LinkedGames(int betType)
        => BetTypeConstants.Happy5LinkedGamesMin <= betType && betType <= BetTypeConstants.Happy5LinkedGamesMax;

    public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);

    public static bool IsJackpotContribution(int betType) => JackpotContributionBetType.Contains(betType);

    public static string GetBetTypeName(int betTypeId)
        => betTypeId == MonitoringBetsOthers
        ? Resources.Others
        : CacheRefServiceHelper.GetBetTypeName(betTypeId);
}