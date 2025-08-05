namespace AlphaPlus.Features.Messages.API.Models.Constants
{
    public static class UserLevel
    {
        private static readonly Dictionary<int, string> Levels = new()
        {
            [Member] = "Member",
            [Agent] = "Agent",
            [Master] = "Master",
            [Super] = "Super"
        };

        public const int Member = 1;
        public const int Agent = 2;
        public const int Master = 3;
        public const int Super = 4;

        public static bool IsMember(int level) => Member == level;

        public static bool IsAgent(int level) => Agent == level;

        public static bool IsMaster(int level) => Master == level;

        public static bool IsSuper(int level) => Super == level;

        public static bool IsNotAgent(int level) => !IsAgent(level);

        public static string GetDisplayName(int level) => Levels[level];
    }
}
