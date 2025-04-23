using AlphaPlus.Core.Models;

namespace AlphaPlus.NetThrottling.Utils
{
    public static class UserSessionFactory
    {
        public static IUserSession Create()
        {
            return UserSession.Current;
        }
    }
}
