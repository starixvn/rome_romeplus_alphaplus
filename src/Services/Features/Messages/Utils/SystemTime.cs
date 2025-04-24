namespace AlphaPlus.Features.Messages.Utils
{
    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? now;

        public static DateTime Now
        {
            get { return now ?? DateTime.Now; }
            internal set { now = value; }
        }
    }
}