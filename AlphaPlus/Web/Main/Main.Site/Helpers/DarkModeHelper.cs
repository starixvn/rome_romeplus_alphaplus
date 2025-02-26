using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Helpers
{
    public static class DarkModeHelper
    {
        public static string GetDarkModeName(IUserSession userSession)
            => userSession.DarkModeName;
    }
}