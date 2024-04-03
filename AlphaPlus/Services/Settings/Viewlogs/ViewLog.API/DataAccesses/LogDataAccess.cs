namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models;

    public interface LogDataAccess
    {
        int GetTotalRecordsOfLogs(ICriteria criteria);

        IEnumerable<Log> GetLogs(ICriteria criteria);
    }
}
