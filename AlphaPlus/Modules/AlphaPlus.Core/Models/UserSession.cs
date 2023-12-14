namespace AlphaPlus.Core.Models;

public static class UserSession
{
    public static IUserSession Current => new UserSessionBase();
}
