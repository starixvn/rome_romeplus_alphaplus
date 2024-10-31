using Fanex.Data.Repository;
using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl
{
    public class LogDataAccessImpl : LogDataAccess
    {
        protected readonly IDynamicRepository dynamicRepository;

        public LogDataAccessImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Task<int> GetTotalRecordsOfLogs(ICriteria criteria)
        {
            return dynamicRepository.GetAsync<int>(criteria);
        }

        public virtual Task<IEnumerable<Log>> GetLogs(ICriteria criteria)
        {
            return dynamicRepository.FetchAsync<Log>(criteria);
        }
    }
}
