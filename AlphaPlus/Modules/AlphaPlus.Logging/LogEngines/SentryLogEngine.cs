using AlphaPlus.Logging.Configurations;
using Fanex.Logging;
using Fanex.Logging.Sentry;
using Sentry;

namespace AlphaPlus.Logging.LogEngines
{
    public class SentryLogEngine : LogEngineFactory
    {
        public ILogEngine Create(string ravenScriptPath = null)
        {
            var sentryOptions = new SentryEngineOptions
            {
                Dsn = new Dsn(AppSettings.SentryBackendUrl),
                FrontEndDsn = new Dsn(AppSettings.SentryUrl)
            };

            return new SentryLogging(sentryOptions, string.IsNullOrWhiteSpace(ravenScriptPath) ? $"{AppSettings.AssetAppPath}/bundles/common/raven.min.js" : ravenScriptPath);
        }
    }
}
