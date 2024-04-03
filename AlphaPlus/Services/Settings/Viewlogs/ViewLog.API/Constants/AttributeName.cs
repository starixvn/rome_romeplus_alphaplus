namespace AlphaPlus.Settings.ViewLogs.API.Constants
{
    using System.Collections.Generic;

    public static class AttributeName
    {
        public const string BetType = "BetType";
        public const string SportType = "SportType";

        public const string MinBet = "MinBet";
        public const string MaxBet = "MaxBet";
        public const string MaxPerMatch = "MaxPerMatch";
        public const string MaxPayoutPerMatch = "MaxPayoutPerMatch";
        public const string MaxPerNumber = "MaxPerNumber";

        public static List<string> IgnoredAttributes => new List<string>
        {
            BetType,
            SportType
        };

        public static Dictionary<string, string> ShortenedAttributes => new Dictionary<string, string>
        {
            [MinBet] = "minb",
            [MaxBet] = "maxb",
            [MaxPerMatch] = "mpm",
            [MaxPayoutPerMatch] = "mppm",
            [MaxPerNumber] = "mpb"
        };
    }
}