using Alpha.Main.Criteria.Theme;
using Fanex.Data.Repository;

namespace Alpha.Main.Services.Impl
{
    public class ThemeServiceImpl(IDynamicRepository repository) : ThemeService
    {
        private readonly IDynamicRepository repository = repository;

        public Task WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location)
        {
            return repository.ExecuteAsync(new LogChangeThemeCriteria(custId, subAccId, isDarkMode, ip, location));
        }
    }
}
