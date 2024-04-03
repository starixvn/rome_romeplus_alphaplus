namespace AlphaPlus.Settings.ViewLogs.API.DataAccesses.Impl
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models;

    public class LoginDataAccessImpl : LogDataAccessImpl, LogDataAccess
    {
        public LoginDataAccessImpl(IDynamicRepository dynamicRepository) : base(dynamicRepository)
        {
        }

        public override IEnumerable<Log> GetLogs(ICriteria criteria)
            => dynamicRepository.Fetch<LoginLog>(criteria);
    }
}