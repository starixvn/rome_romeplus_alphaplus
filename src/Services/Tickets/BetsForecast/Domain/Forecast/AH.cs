namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class AH(
    int matchId,
    string eventName,
    string eventScore,
    bool hasDataMatch,
    int betType,
    string betTypeName,
    int currentScore,
    bool hasData,
    IEnumerable<decimal> hdpou,
    IEnumerable<int> allScores,
    IEnumerable<int> forecastScores)
{
    public int MatchId { get; } = matchId;

    public string EventName { get; } = eventName;

    public string EventScore { get; } = eventScore;

    public bool HasDataMatch { get; } = hasDataMatch;

    public int BetType { get; } = betType;

    public string BetTypeName { get; } = betTypeName;

    public int CurrentScore { get; } = currentScore;

    public bool HasData { get; } = hasData;

    public IEnumerable<decimal> HDPOU { get; } = hdpou;

    public IEnumerable<int> AllScores { get; } = allScores;

    public IEnumerable<int> ForecastScores { get; } = forecastScores;
}
