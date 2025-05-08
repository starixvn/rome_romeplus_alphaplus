namespace AlphaPlus.BetLists.LastBets;

[Serializable]
public record AddFilterParam(
    int CustId,
    int SubAccId,
    string FilterName,
    bool IsDefault,
    bool IsNotify,
    string BetType,
    int Stake,
    SportXml[] Sports,
    byte LiveStatus,
    string InputList,
    string InputListMembers,
    string MyWatchlist,
    string MyWatchlistMembers,
    byte FilterByMembers,
    string LanguageKey,
    string IP,
    string SiteName,
    byte RoleId) : UpdateFilterParam(CustId, SubAccId, 0, FilterName, IsDefault, IsNotify, LanguageKey, IP, SiteName);
