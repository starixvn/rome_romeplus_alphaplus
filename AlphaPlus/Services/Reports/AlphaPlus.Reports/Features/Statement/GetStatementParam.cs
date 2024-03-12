namespace AlphaPlus.Reports.Features.Statement;

public record GetStatementParam(int CustId, DateTime FromDate, DateTime ToDate, string SiteName);
