namespace AlphaPlus.BetLists.RiskAnalysisIPMatching;

public record RiskAnalysisIPMatchingParam(
    int CustId,
    string IP,
    DateTime Date,
    bool DisplayVNIP,
    string SiteName);
