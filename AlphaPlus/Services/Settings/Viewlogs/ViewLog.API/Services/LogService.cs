namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public interface LogService
    {
        IEnumerable<LogActionType> GetLogActionTypes();

        string? GetLogActionName(int actionId, string language);

        PagingLogs<Log> GetPagingLogs(LogParam logParam);
    }
}