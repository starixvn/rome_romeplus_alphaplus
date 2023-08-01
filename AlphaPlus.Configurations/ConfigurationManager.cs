namespace AlphaPlus.Configurations
{
    public static class ConfigurationManager
    {
        private static IConfigurationHelper _configurationHelper;

        public static void Init(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public static T GetValue<T>(string key, T defaultValue = default)
        {
            return _configurationHelper.GetValue(key, defaultValue);
        }
    }
}