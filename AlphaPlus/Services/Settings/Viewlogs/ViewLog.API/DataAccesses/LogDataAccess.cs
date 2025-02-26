using Fanex.Data.Repository;
using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses
{
    public interface LogDataAccess
    {
        Task<int> GetTotalRecordsOfLogs(ICriteria criteria);

        Task<IEnumerable<Log>> GetLogs(ICriteria criteria);
    }
}
