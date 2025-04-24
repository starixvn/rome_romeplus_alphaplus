namespace AlphaPlus.BetLists.LastBets;

[Serializable]
public record DeleteFilterParam(
    int CustId,
    int SubAccId,
    int FilterId,
    string LanguageKey,
    string IP,
    string SiteName);
