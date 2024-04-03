namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models;

    public class LogDataAccessImpl : LogDataAccess
    {
        protected readonly IDynamicRepository dynamicRepository;

        public LogDataAccessImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public int GetTotalRecordsOfLogs(ICriteria criteria)
        {
            return dynamicRepository.Get<int>(criteria);
        }

        public virtual IEnumerable<Log> GetLogs(ICriteria criteria)
        {
            return dynamicRepository.Fetch<Log>(criteria);
        }
    }
}
