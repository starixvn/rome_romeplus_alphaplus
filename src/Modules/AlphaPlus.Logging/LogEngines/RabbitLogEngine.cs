using AlphaPlus.Logging.Configurations;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;

namespace AlphaPlus.Logging.LogEngines
{
    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create(string ravenScriptPath = null)
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
