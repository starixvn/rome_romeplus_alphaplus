namespace Alpha.Tickets.Infrastructure.Params.BetList;

public record RunningBetListParam(
    int CustId,
    string ProductIds,
    DateTime FromDate,
    DateTime ToDate,
    string Username,
    bool GetAll = false);