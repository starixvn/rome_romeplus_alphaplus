using System.Runtime.Serialization;

namespace AlphaPlus.PartialUM.Exceptions;

[Serializable]
public class PartialUMException : Exception
{
    protected PartialUMException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public PartialUMException(string startTime, string endTime)
        : base($"We're updating this function for a better experience. Thus, this function is turned off temporarily ({startTime} - {endTime} (GMT+8)). Please try again later and sorry for the inconvenience.")
    {
        StartTime = startTime;
        EndTime = endTime;
        ErrorCode = 9999;
        ErrorData = new Dictionary<string, object>
        {
            [nameof(startTime)] = startTime,
            [nameof(endTime)] = endTime
        };

    }

    public string StartTime { get; }

    public string EndTime { get; }

    public int ErrorCode { get; }

    public IDictionary<string, object> ErrorData { get; }

    // public int ErrorCode { get; } = 9999;

    // public IDictionary<string, string> ErrorData { get; } = new Dictionary<string, string>
    // {
    //     [nameof(StartTime)] = StartTime,
    //     [nameof(EndTime)] = EndTime
    // };
}
