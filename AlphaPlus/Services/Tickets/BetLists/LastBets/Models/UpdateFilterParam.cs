namespace AlphaPlus.BetLists.LastBets;

[Serializable]
public record UpdateFilterParam(
    int CustId,
    int SubAccId,
    int FilterId,
    string FilterName,
    bool IsDefault,
    string LanguageKey,
    string IP,
    string SiteName) : DeleteFilterParam(CustId, SubAccId, FilterId, LanguageKey, IP, SiteName);
