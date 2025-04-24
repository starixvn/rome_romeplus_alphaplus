using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;

namespace AlphaPlus.Logging
{
    public class ExceptionHandlerAdapter
    {
        public virtual LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration) => ExceptionHandler.UseConfig(loggingConfiguration);
    }
}
