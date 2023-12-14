using Alpha.Main.Criteria;
using Fanex.Data.Repository;
using Alpha.Main.Params;

namespace Alpha.Main.Services.Impl
{
    public class LogoutServiceImpl(IDynamicRepository dynamicRepository) : LogoutService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task Logout(LogoutParam logoutParam)
            => dynamicRepository.ExecuteAsync(new LogoutCommand(logoutParam));
    }
}
