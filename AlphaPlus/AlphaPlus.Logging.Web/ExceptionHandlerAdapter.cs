using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Web;

namespace AlphaPlus.Logging.Web
{
    public class ExceptionHandlerAdapter : IExceptionHandlerAdapter
    {
        public LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration) => ExceptionHandler.UseConfig(loggingConfiguration);
    }
}
