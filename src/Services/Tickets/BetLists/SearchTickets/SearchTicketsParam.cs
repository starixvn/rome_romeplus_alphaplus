namespace AlphaPlus.BetLists.SearchTickets;

public record SearchTicketsParam(
    long RefNo,
    int UplineId,
    byte UplineLevelId,
    bool DisplayVNIP,
    int SuperId,
    string System,
    string SiteName);