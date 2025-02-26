using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface LogService
    {
        Task<IEnumerable<LogActionType>> GetLogActionTypes();

        Task<string> GetLogActionName(int actionId, string language);

        Task<PagingLogs<Log>> GetPagingLogs(LogParam logParam);
    }
}