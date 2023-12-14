using Fanex.Logging;

namespace AlphaPlus.Logging.LogEngines
{
    public interface LogEngineFactory
    {
        ILogEngine Create(string ravenScriptPath = null);
    }
}
