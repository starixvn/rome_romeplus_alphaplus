namespace AlphaPlus.BetLists.DailyStatement;

public record DailyStatementParam(
    int CustId,
    DateTime WinLossDate,
    int TransType,
    bool DisplayVNIP,
    int SuperId,
    string SiteName);