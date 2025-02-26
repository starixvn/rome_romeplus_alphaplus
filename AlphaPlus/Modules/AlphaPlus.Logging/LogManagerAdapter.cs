using Fanex.Logging;

namespace AlphaPlus.Logging
{
    public class LogManagerAdapter : LogManager
    {
        public new virtual LogManager SetDefaultLogCategory(string defaultCategory)
            => LogManager.SetDefaultLogCategory(defaultCategory);
    }
}
