namespace Alpha.Tickets.Infrastructure.Models.BetList;

public record OddsMixParlay
{
    public long TransId { get; init; }

    public decimal Odds { get; init; }

    public decimal MasterOdds { get; init; }
}
