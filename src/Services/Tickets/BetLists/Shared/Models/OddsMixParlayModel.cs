namespace AlphaPlus.BetLists.Shared.Models;

public class OddsMixParlayModel
{
    public long TransId { get; init; }

    public decimal Odds { get; init; }

    public decimal MasterOdds { get; init; }
}