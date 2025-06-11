namespace Alpha.Handbook.Demo.Helpers
{
    using Alpha.Handbook.Demo.Constants;
    using Alpha.Handbook.Demo.Configurations;

    public static class AppSettingsHelper
    {
        public static string ContentAPIEnvironment => ConfigurationManager.GetValue<string>("ContentAPI:Environment");
        public static string ContentAPIUrl => ConfigurationManager.GetValue<string>("ContentAPIUrl");
        public static string ContentAPIToken => ConfigurationManager.GetValue<string>("ContentAPIToken");
        public static string SiteLabel => ConfigurationManager.GetValue<string>("SiteLabel");   
        public static string RoleId => ConfigurationManager.GetValue<string>("RoleId");
        
        public static string SessionLanguageKey { get; set;} = LanguageConsts.EnglishKey;
        public static bool SupportVietnamese => true;
    }
}