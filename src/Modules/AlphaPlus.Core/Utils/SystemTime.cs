namespace AlphaPlus.Core.Utils;

public static class SystemTime
{
    public static DateTime Yesterday => Today.AddDays(-1);

    public static DateTime Today => Now.Date;

    [ThreadStatic]
    private static DateTime now;
    public static DateTime Now
    {
        get
        {
            if (now == default)
            {
                now = DateTime.Now;
            }

            return now;
        }
    }
}