using AlphaPlus.User.Models;

namespace Alpha.NetThrottling.Utils
{

    public static class UserSessionFactory
    {
        public static IUserSession Create()
        {
            return UserSession.Current;
        }
    }
}
