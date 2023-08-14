namespace AlphaPlus.SecurityCode.Web
{
    public static class SecurityCodeSessionManager
    {
        public static SecurityCodeSession SecurityCodeSession => new SecurityCodeSessionImpl();
    }
}