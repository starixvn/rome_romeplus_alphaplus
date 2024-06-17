namespace AlphaPlus.BetLists.LastBets;

public class Sport
{
    public int Id { get; init; }

    public string Name { get; init; }

    public IEnumerable<League> Leagues { get; init; }
}
