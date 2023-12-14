namespace Alpha.Main.Site.Helpers
{
    using AlphaPlus.Core.Models;

    public static class DarkModeHelper
    {
        public static string GetDarkModeName(IUserSession userSession)
            => userSession.DarkModeName;
    }
}