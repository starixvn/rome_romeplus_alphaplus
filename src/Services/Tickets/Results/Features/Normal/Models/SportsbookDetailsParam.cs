namespace AlphaPlus.Results.Features.Normal;

public record SportsbookDetailsParam(
    bool IsViewResultFromBetList,
    int BetType,
    int MatchId,
    long BetId,
    string Transdesc,
    int Hdp2);