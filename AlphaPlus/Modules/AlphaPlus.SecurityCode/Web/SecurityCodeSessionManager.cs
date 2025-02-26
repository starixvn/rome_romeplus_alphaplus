namespace AlphaPlus.SecurityCode.Web;

public static class SecurityCodeSessionManager
{
    static SecurityCodeSessionManager()
    {
        SecurityCodeSession ??= new SecurityCodeSession();
    }

    public static ISecurityCodeSession SecurityCodeSession { get; set; }
}
