namespace Alpha.Tickets.Infrastructure.Models.Forecast;

public record ScoreMap
{
    public int TypeId { get; init; }

    public int EventType { get; init; }

    public string FirstId { get; init; }

    public string InfoId { get; init; }
}
