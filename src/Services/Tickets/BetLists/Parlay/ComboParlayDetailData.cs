namespace AlphaPlus.BetLists.Parlay;

public class ComboParlayDetailData(
    IEnumerable<ComboParlayDetail> comboParlayDetails,
    IEnumerable<MatchDetail> matchDetails,
    decimal outstanding,
    bool isParlayBoost)
{
    public IEnumerable<ComboParlayDetail> ComboParlayDetails { get; } = comboParlayDetails;

    public IEnumerable<MatchDetail> MatchDetails { get; } = matchDetails;

    public decimal Outstanding { get; } = outstanding;

    public bool IsParlayBoost { get; } = isParlayBoost;
}