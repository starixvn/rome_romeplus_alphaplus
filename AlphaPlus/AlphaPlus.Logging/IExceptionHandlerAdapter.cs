namespace AlphaPlus.Logging
{
    using Fanex.Logging.Extensions;

    public interface IExceptionHandlerAdapter
    {
        LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration);
    }
}
