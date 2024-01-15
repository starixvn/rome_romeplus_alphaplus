namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapBetType(
    int betType,
    string betTypeName,
    string followedBetTypes,
    IEnumerable<string> followedBetTypeNames)
{
    public int BetType { get; init; } = betType;

    public string BetTypeName { get; set; } = betTypeName;

    public string FollowedBetTypes { get; init; } = followedBetTypes;

    public IEnumerable<string> FollowedBetTypeNames { get; init; } = followedBetTypeNames;
}
