namespace AlphaPlus.Results.Features.Normal;

public class GetSportsbookResultBetListCriteria(
    int matchId,
    int betType,
    long betId,
    string transdesc) : GetSportsbookResultDetailsCriteria(matchId)
{
    public int BetType { get; } = betType;

    public long BetId { get; } = betId;

    public string Transdesc { get; } = transdesc;

    public override string GetSettingKey() => "Age_BetList_SelMatchResult";
}