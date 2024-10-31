namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models;

    public interface LogDataAccess
    {
        Task<int> GetTotalRecordsOfLogs(ICriteria criteria);

        Task<IEnumerable<Log>> GetLogs(ICriteria criteria);
    }
}
