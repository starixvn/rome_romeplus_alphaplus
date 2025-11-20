namespace AlphaPlus.BetLists.MoveStatementHistory;

public record MoveStatementHistoryParam(int CustId, long TransId, bool DisplayVNIP, int SuperId, string System, string SiteName);
