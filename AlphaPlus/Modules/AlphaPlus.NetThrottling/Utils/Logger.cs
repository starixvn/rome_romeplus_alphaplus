using Fanex.Logging.Extensions.Web;

namespace AlphaPlus.NetThrottling.Utils
{
    public interface ILogger
    {
        void Error(Exception ex);
    }

    public class Logger : ILogger
    {
        public static readonly ILogger Default = new Logger();

        private readonly int LogTimeSpanMinutes = 5;
        private DateTime LastLogTime = DateTime.MinValue;
        private List<string> LastMessages = [];

        public void Error(Exception ex)
        {
            if ((DateTime.Now - LastLogTime).Minutes >= LogTimeSpanMinutes)
            {
                lock (LastMessages)
                {
                    LastLogTime = DateTime.Now;
                    LastMessages = new List<string>();
                }
            }

            if (LastMessages.Contains(ex.Message))
            {
                return;
            }

            lock (LastMessages)
            {
                LastMessages.Add(ex.Message);
                Handle(ex);
            }
        }

        public static void Handle(Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
    }
}
