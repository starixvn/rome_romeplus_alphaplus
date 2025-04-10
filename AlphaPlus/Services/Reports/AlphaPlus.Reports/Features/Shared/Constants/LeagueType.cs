namespace AlphaPlus.Reports.Features.Shared.Constants;

public static class LeagueType
{
    public const byte Self = 0;

    public const byte Match3rd_League3rd = 1;

    public const byte VSouright_League3rd = 2;

    public static bool IsLeague3rd(int leagueType)
        => leagueType == Match3rd_League3rd || leagueType == VSouright_League3rd;
}