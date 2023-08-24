namespace AlphaPlus.Logging.LogEngines
{
    using AlphaPlus.Logging.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;

    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create(string ravenScriptPath = null)
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
