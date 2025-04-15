namespace AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;

public record TransferToDownlineHistoryParam(
    int CustId,
    string UserName,
    DateTime FromDate,
    DateTime ToDate,
    byte LevelType,
    int PageSize,
    int PageIndex);