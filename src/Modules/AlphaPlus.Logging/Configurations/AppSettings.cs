using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Logging.Configurations
{
    public static class AppSettings
    {
        public static string LoggingDefaultCategory => ConfigurationManager.GetValue<string>("Fanex.Logging:DefaultCategory");

        public static string LogLibs => ConfigurationManager.GetValue<string>("LogLibs");

        public static string RabbitMQConnectionStrings => ConfigurationManager.GetValue<string>("Fanex.Logging:RabbitMQConnectionStrings");

        public static string SentryBackendUrl => ConfigurationManager.GetValue<string>("Fanex.Logging:SentryBackendUrl");

        public static string SentryUrl => ConfigurationManager.GetValue<string>("Fanex.Logging:SentryUrl");

        public static string AssetAppPath => ConfigurationManager.GetValue<string>("AssetAppPath");

    }
}
