using AlphaPlus.User.Models.Helpers;

namespace AlphaPlus.User.Models
{
    public static class UserSession
    {
        private static IHttpContextHelper _httpContextHelper;

        public static void Init(IHttpContextHelper httpContextHelper)
        {
            _httpContextHelper = httpContextHelper;
        }

        public static IUserSession Current => new UserSessionBase(_httpContextHelper);
    }
}