namespace AlphaPlus.BetLists.LastBets;

public record SavedFilter(
    int FilterId,
    string FilterName,
    bool IsDefault,
    bool IsNotify,
    string BetTypes,
    int Stake,
    byte EventStatus,
    string SportTypes,
    string Leagues,
    string Members,
    byte FilterByMembers);
