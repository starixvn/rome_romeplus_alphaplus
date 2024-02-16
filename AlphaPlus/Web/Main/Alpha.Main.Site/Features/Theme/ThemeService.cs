using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.Theme
{
    public interface IThemeService
    {
        Task WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location);
    }

    public class ThemeService(IDynamicRepository repository) : IThemeService
    {
        private readonly IDynamicRepository repository = repository;

        public Task WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location)
        {
            return repository.ExecuteAsync(new LogChangeThemeCriteria(custId, subAccId, isDarkMode, ip, location));
        }
    }
}