namespace Alpha.Main.Services
{
    using Params;

    public interface LogoutService
    {
        Task Logout(LogoutParam logoutParam);
    }
}
