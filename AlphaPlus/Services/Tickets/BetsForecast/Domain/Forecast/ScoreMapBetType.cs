namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapBetType(int betType, string betTypeName, string followedBetTypes)
{
    public int BetType { get; init; } = betType;

    public string BetTypeName { get; set; } = betTypeName;

    public string FollowedBetTypes { get; init; } = followedBetTypes;
}