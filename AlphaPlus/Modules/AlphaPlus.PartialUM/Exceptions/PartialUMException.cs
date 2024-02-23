namespace AlphaPlus.PartialUM.Exceptions;

[Serializable]
public class PartialUMException(string StartTime, string EndTime) : Exception
{
    public int ErrorCode { get; } = 9999;

    public IDictionary<string, string> ErrorData { get; } = new Dictionary<string, string>
    {
        [nameof(StartTime)] = StartTime,
        [nameof(EndTime)] = EndTime
    };
}
