namespace AlphaPlus.PartialUM.Exceptions;

public class PartialUMException(string startTime, string endTime) : Exception
{
    public const int ErrorCode = 9999;

    public string StartTime { get; } = startTime;

    public string EndTime { get; } = endTime;
}
