namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class Match
{
    public int MatchId { get; init; }

    public string MatchCode { get; init; }

    public int HomeId { get; init; }

    public string HomeName { get; set; }

    public int AwayId { get; init; }

    public string AwayName { get; set; }

    public virtual string Event => $"{HomeName} - {AwayName}";
}