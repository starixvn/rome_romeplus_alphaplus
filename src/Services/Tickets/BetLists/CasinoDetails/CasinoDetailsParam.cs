namespace AlphaPlus.BetLists.CasinoDetails;

public record CasinoDetailsParam(
    long RefNo,
    int CustId,
    byte TransType,
    int ProductId,
    int PageIndex,
    int PageSize,
    bool DisplayVNIP);
