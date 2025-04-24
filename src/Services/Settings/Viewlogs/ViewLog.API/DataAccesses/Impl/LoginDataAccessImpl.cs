using Fanex.Data.Repository;
using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl
{
    public class LoginDataAccessImpl : LogDataAccessImpl, LogDataAccess
    {
        public LoginDataAccessImpl(IDynamicRepository dynamicRepository) : base(dynamicRepository)
        {
        }

        public override Task<IEnumerable<Log>> GetLogs(ICriteria criteria)
            => dynamicRepository.FetchAsync<Log>(criteria);
    }
}