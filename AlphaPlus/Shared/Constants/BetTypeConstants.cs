public static class BetTypeConstants
{
    public const int Handicap = 1;
    public const int OddEven = 2;
    public const int OverUnder = 3;
    public const int _1X2 = 5;
    public const int _1stHandicap = 7;
    public const int _1stOverUnder = 8;
    public const int MixParlay = 9;
    public const int Outright = 10;
    public const int _1stOddEven = 12;
    public const int _1st1X2 = 15;
    public const int HTFT = 16;
    public const int Moneyline = 20;
    public const int _1stMoneyline = 21;
    public const int DrawNoBet = 25;
    public const int CombineMixParlay = 29;
    public const int BetBuilder = 38;
    public const int HomeNoBet = 121;
    public const int AwayNoBet = 122;
    public const int DrawNoDraw = 123;
    public const int MixParlayBetRadarVirtualSports = 2799;

    public const int VirtualHorseRacing_Win = 1231;
    public const int VirtualHorseRacing_Place = 1232;
    public const int VirtualHorseRacing_WinPlace = 1233;

    public const int JackpotContribution = 2198;
    public const int JackpotContribution_Mobile = 2398;
    public const int JackpotContribution_Tablet = 2998;
    public const int PPJackpotContribution = 4297;
    public const int SGJackpotContribution = 4599;

    public const int Happy5LinkedGamesMin = 3601;
    public const int Happy5LinkedGamesMax = 3610;

    public const int LeapGamingMin = 5301;
    public const int LeapGamingMax = 5399;

    public static bool IsOutright(int betType) => betType == Outright;

    public static bool IsJackpotContribution(int betType)
        => JackpotContribution == betType
        || JackpotContribution_Mobile == betType
        || JackpotContribution_Tablet == betType
        || PPJackpotContribution == betType
        || SGJackpotContribution == betType;
}